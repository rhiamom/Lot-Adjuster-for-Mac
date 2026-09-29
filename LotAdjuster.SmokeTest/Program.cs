/**************************************************************************
 *   LotAdjuster for Mac                                                  *
 *   LotAdjuster © 2008-2013 Mootilda; portions © 2006 Andi8104           *
 *   macOS port © 2026 GramzeSweatshop (rhiamom@mac.com)                  *
 *   Ported with Claude (Anthropic)                                       *
 *   GPL v2 or later. See Licences/GPL-LICENSE.txt                        *
 *                                                                        *
 *   Headless harness. Drives Mootilda's own form logic (LotExpander.cs,  *
 *   unchanged) through the headless controls, the way a user clicked it. *
 *************************************************************************/

// LotAdjuster smoke test.
//
//   list   <HOOD>                         lots in a neighborhood package
//   adjust <HOOD> <lot#> [--front N] [--back N] [--left N] [--right N]
//   all    <HOOD>                         her RunTests: every built lot in every
//                                         package of the hood, +4/+6/+3/+7
//   backroom <HOOD>                       built lots with a free strip (no lot,
//                                         no road) one hood tile behind them
//
// <HOOD> is a hood code (N001) for its main package, or CODE/<package file>
// for a subhood. Options: --out DIR (work folder), --in-place (edit the real
// hood; never the default).
//
// Unless --in-place is given, the whole hood folder is first copied to a work
// folder and only the copy is changed.

using System.Drawing;
using LotExpander;
using SimPe.Interfaces.Files;
using SimPe.Packages;

namespace LotAdjuster.SmokeTest;

internal static class Program
{
    private const uint LOT = 0x6C589723;
    private const uint MOBJT = 0x6F626A74;
    private const uint DESC = 0x0BF999E7;

    private static readonly List<string> Messages = new();

    private static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("usage: list|adjust|all <HOOD> [lot#] [--front N --back N --left N --right N] [--out DIR] [--in-place]");
            return 2;
        }

        // Her message boxes: print, answer with the default button.
        MessageBox.Handler = (owner, m) =>
        {
            string line = $"[{m.Caption}] {m.Text.Replace("\n", " ")} -> {m.DefaultResult}";
            Messages.Add(line);
            Console.WriteLine("    message: " + line);
            return m.DefaultResult;
        };

        string mode = args[0];
        string hoodArg = args[1];
        bool inPlace = args.Contains("--in-place");
        string? outDir = Opt(args, "--out");

        string? nbRoot = SimsPaths.NeighborhoodsFolder;
        if (nbRoot == null) { Console.Error.WriteLine("Sims 2 Neighborhoods folder not found."); return 1; }

        string code = hoodArg.Split('/')[0];
        string hoodDir = Path.Combine(nbRoot, code);
        if (!Directory.Exists(hoodDir)) { Console.Error.WriteLine($"No hood folder {hoodDir}"); return 1; }

        if (mode != "list" && mode != "backroom" && !inPlace)
        {
            outDir ??= Path.Combine(Path.GetTempPath(), "lotadjuster-smoke",
                $"{code}-{DateTime.Now:yyyyMMdd-HHmmss}");
            string copy = Path.Combine(outDir, code);
            CopyDir(hoodDir, copy);
            Console.WriteLine($"Working on a copy: {copy}");
            hoodDir = copy;
        }

        string package = hoodArg.Contains('/')
            ? Path.Combine(hoodDir, hoodArg.Split('/', 2)[1])
            : Path.Combine(hoodDir, code + "_Neighborhood.package");

        return mode switch
        {
            "list" => List(package),
            "adjust" => Adjust(package, args),
            "all" => All(hoodDir),
            "backroom" => Backroom(package),
            _ => 2,
        };
    }

    private static string? Opt(string[] args, string name)
    {
        int i = Array.IndexOf(args, name);
        return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
    }

    private static int OptInt(string[] args, string name) => int.TryParse(Opt(args, name), out int v) ? v : 0;

    private static void CopyDir(string from, string to)
    {
        Directory.CreateDirectory(to);
        foreach (string f in Directory.GetFiles(from))
            System.IO.File.Copy(f, Path.Combine(to, Path.GetFileName(f)), true);
        foreach (string d in Directory.GetDirectories(from))
            CopyDir(d, Path.Combine(to, Path.GetFileName(d)));
    }

    private static PrimaryForm NewForm()
    {
        var form = new PrimaryForm();
        form.Show();    // Load + Shown, as WinForms did
        return form;
    }

    private static IEnumerable<R_DESC> Lots(PrimaryForm form)
    {
        foreach (object o in form.Liste.Items)
            if (o is R_DESC d) yield return d;
    }

    private static string LotPackagePath(string hoodPackage, uint instance)
    {
        string name = Path.GetFileName(hoodPackage);
        string prefix = name.Substring(0, name.LastIndexOf('_'));
        return Path.Combine(Path.GetDirectoryName(hoodPackage)!, "Lots", $"{prefix}_Lot{instance}.package");
    }

    private static bool IsBuilt(string hoodPackage, uint instance)
    {
        string p = LotPackagePath(hoodPackage, instance);
        if (!System.IO.File.Exists(p)) return false;
        var pkg = SimPe.Packages.File.LoadFromFile(p);
        return pkg.FindFile(MOBJT, 0, 0xFFFFFFFF, 0) != null;
    }

    private static int List(string package)
    {
        var form = NewForm();
        form.OpenNeighborhood(package);
        Console.WriteLine($"{form.Title.Text}  ({package})");
        foreach (R_DESC d in Lots(form))
            Console.WriteLine($"  Lot{d.Instance,-4} {d.Width}x{d.Height} at ({d.Top},{d.Left}) orient {d.Orientation} " +
                              $"U10 {d.U10} U11 {d.U11} type {d.LotType} {(IsBuilt(package, d.Instance) ? "" : "(empty) ")}| {d}");
        return 0;
    }

    private sealed record Result(bool Ok, string Title, string Explanation, int[] Expected);

    // One pass through her screens: Lot list -> Next -> yards -> Finish.
    private static Result RunOne(string package, uint instance, int front, int back, int left, int right)
    {
        var form = NewForm();
        form.OpenNeighborhood(package);
        R_DESC lot = Lots(form).First(l => l.Instance == instance);
        form.Liste.SelectedItem = lot;
        if (!form.NextButton.PerformClick() || form.CurrentScreen != PrimaryForm.ScreenExpansion)
            return new Result(false, form.Title.Text, "could not open the lot's size screen", Array.Empty<int>());

        try
        {
            if (left != 0) form.LeftYard.Value = left;
            if (right != 0) form.RightYard.Value = right;
            if (front != 0) form.FrontYard.Value = front;
            if (back != 0) form.BackYard.Value = back;
        }
        catch (ArgumentOutOfRangeException)
        {
            return new Result(false, form.Title.Text,
                $"size out of range (max {form.WidthMax.Text}x{form.HeightMax.Text})", Array.Empty<int>());
        }

        int[] expected = { int.Parse(form.WidthNew.Text), int.Parse(form.HeightNew.Text) };
        Console.WriteLine($"    {form.WidthOld.Text}x{form.HeightOld.Text} -> {expected[0]}x{expected[1]}");
        if (!form.NextButton.PerformClick())
            return new Result(false, form.Title.Text, "Finish disabled: " + form.SizeError.Text, expected);

        bool aborted = form.Title.ForeColor == Color.Red || form.CurrentScreen != PrimaryForm.ScreenFinal;
        return new Result(!aborted, form.Title.Text, form.Explanation.Text, expected);
    }

    // Re-read both packages from disk and check the new size landed.
    private static bool Verify(string package, uint instance, int[] expected)
    {
        var hood = SimPe.Packages.File.LoadFromFile(package);
        var d = new R_DESC(hood, hood.FindFile(DESC, 0, 0xFFFFFFFF, instance));
        var lotPkg = SimPe.Packages.File.LoadFromFile(LotPackagePath(package, instance));
        var l = new R_LOT(lotPkg, lotPkg.FindFiles(LOT).Single());

        var want = expected.OrderBy(x => x).ToArray();
        var descSize = new[] { d.Width * 10, d.Height * 10 }.OrderBy(x => x).ToArray();
        var lotSize = new[] { l.Width, l.Height }.OrderBy(x => x).ToArray();
        bool bkp = System.IO.File.Exists(Path.ChangeExtension(package, ".bkp"))
                && System.IO.File.Exists(Path.ChangeExtension(LotPackagePath(package, instance), ".bkp"));
        bool ok = want.SequenceEqual(descSize) && want.SequenceEqual(lotSize) && bkp;
        Console.WriteLine($"    verify: DESC {d.Width}x{d.Height} tiles, LOT {l.Width}x{l.Height}, backups {(bkp ? "yes" : "NO")} -> {(ok ? "OK" : "MISMATCH")}");
        return ok;
    }

    private static int Adjust(string package, string[] args)
    {
        if (args.Length < 3 || !uint.TryParse(args[2].Replace("Lot", ""), out uint inst))
        {
            Console.Error.WriteLine("adjust needs a lot number (see: list)");
            return 2;
        }
        var r = RunOne(package, inst, OptInt(args, "--front"), OptInt(args, "--back"),
                       OptInt(args, "--left"), OptInt(args, "--right"));
        Console.WriteLine($"  {(r.Ok ? "DONE" : "ABORTED")}: {r.Title} | {r.Explanation}");
        return r.Ok && Verify(package, inst, r.Expected) ? 0 : 1;
    }

    // Lot rectangles: Top..Top+Width-1 along x, Left..Left+Height-1 along y.
    // Orientation = front edge: 0 row x=Top, 2 row x=Top+W-1, 3 column y=Left,
    // 1 column y=Left+H-1. Adding back yard grows the lot on the opposite side
    // (her FixLotInNeighborhood), so check that strip.
    private static IEnumerable<(int x, int y)> BackStrip(R_DESC d) => d.Orientation switch
    {
        0 => Enumerable.Range(d.Left, d.Height).Select(y => (d.Top + d.Width, y)),
        2 => Enumerable.Range(d.Left, d.Height).Select(y => (d.Top - 1, y)),
        3 => Enumerable.Range(d.Top, d.Width).Select(x => (x, d.Left + d.Height)),
        _ => Enumerable.Range(d.Top, d.Width).Select(x => (x, d.Left - 1)),
    };

    private static int Backroom(string package)
    {
        var hood = SimPe.Packages.File.LoadFromFile(package);
        var nhtr = new HoodReplace.R_NHTR(hood, hood.FindFile(0xABD0DC63, 0, 0xFFFFFFFF, 0));
        var roads = new HashSet<(int, int)>();
        byte[] ra = nhtr.Roads;
        for (int i = 0; i + 124 <= ra.Length; i += 124)
            roads.Add(((int)(BitConverter.ToSingle(ra, i + 1) / 10), (int)(BitConverter.ToSingle(ra, i + 5) / 10)));
        var descs = hood.FindFiles(DESC).Select(p => new R_DESC(hood, p)).ToList();
        var taken = new HashSet<(int, int)>();
        foreach (var d in descs)
            for (int x = d.Top; x < d.Top + d.Width; x++)
                for (int y = d.Left; y < d.Left + d.Height; y++)
                    taken.Add((x, y));
        Console.WriteLine($"{roads.Count} road squares, {descs.Count} lots");
        foreach (var d in descs.OrderBy(d => d.Instance))
        {
            if (!IsBuilt(package, d.Instance)) continue;
            var strip = BackStrip(d).ToList();
            int lots = strip.Count(taken.Contains), road = strip.Count(roads.Contains);
            bool edge = strip.Any(p => p.x < 0 || p.y < 0 || p.x > 127 || p.y > 127);
            bool max = Math.Max(d.Width, d.Height) >= 6;
            Console.WriteLine($"  Lot{d.Instance,-4} {(lots == 0 && road == 0 && !edge ? "FREE " : "     ")} " +
                              $"lot squares {lots}, road squares {road}{(edge ? ", off map" : "")}{(max ? ", at max size" : "")} | {d}");
        }
        return 0;
    }

    // Her RunTests(): every built lot in every package, expanded by her
    // Test_Setup amounts (left 4, right 6, front 3, back 7), skipping a
    // direction already at the 60-tile maximum.
    private static int All(string hoodDir)
    {
        int ok = 0, failed = 0, skipped = 0;
        foreach (string package in Directory.GetFiles(hoodDir, "*.package").OrderBy(p => p))
        {
            var pkg = SimPe.Packages.File.LoadFromFile(package);
            IPackedFileDescriptor[] descs = pkg.FindFiles(DESC);
            if (descs.Length == 0) continue;
            Console.WriteLine(Path.GetFileName(package));
            var probe = NewForm();
            probe.OpenNeighborhood(package);
            foreach (R_DESC lot in Lots(probe).ToList())
            {
                if (!IsBuilt(package, lot.Instance)) { skipped++; continue; }
                Console.WriteLine($"  Lot{lot.Instance} {lot}");
                bool wideMax = lot.Width * 10 >= 60, deepMax = lot.Height * 10 >= 60;
                bool swap = lot.U11 % 2 == 0;   // her lot-space width/height swap
                bool lrMax = swap ? deepMax : wideMax, fbMax = swap ? wideMax : deepMax;
                var r = RunOne(package, lot.Instance,
                    fbMax ? 0 : 3, fbMax ? 0 : 7, lrMax ? 0 : 4, lrMax ? 0 : 6);
                if (r.Ok && Verify(package, lot.Instance, r.Expected)) ok++;
                else { failed++; Console.WriteLine($"    FAILED: {r.Title} | {r.Explanation}"); }
            }
        }
        Console.WriteLine($"\n{ok} adjusted OK, {failed} failed, {skipped} empty lots skipped, {Messages.Count} messages.");
        return failed == 0 ? 0 : 1;
    }
}
