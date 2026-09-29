/***************************************************************************
 *   macOS port © 2026 GramzeSweatshop                                     *
 *   GNU GPLv2 or later, see LICENSE.                                      *
 ***************************************************************************/
// The headless half of Mootilda's PrimaryForm. It stands in for her
// LotExpander.Designer.cs, which is pure WinForms layout and does not come
// to macOS. Her LotExpander.cs is compiled UNCHANGED beside it.
//
// InitializeComponent below declares the same controls under the same names
// (as stand-ins from WinFormsHeadless.cs), applies the same starting values
// (her resx strings and Visible/Enabled flags via PrimaryForm.resx, plus the
// few values her Designer set in code), and wires exactly the same event
// handlers her Designer wired, in the same order. Layout (Location, Size,
// Font, TabIndex...) is left out.
//
// The controls are public so a host (the Avalonia app, the SmokeTest) can
// read and set them. A host drives her form the way a user did: set
// checkboxes and yard values, then PerformClick() her buttons.

using System;
using System.Globalization;
using System.Resources;
using SimPe.Packages;

namespace LotExpander
{
    public partial class PrimaryForm
    {
        #region Controls (names and types as in LotExpander.Designer.cs)
        public ListBox Liste;
        public Label Title;
        public Button NextButton;
        public Button BackButton;
        public Button AdvancedButton;
        public Button Defaults;
        public GroupBox LotProperties;
        public GroupBox AdvancedFeatures;
        public Panel ClassValuePanel;
        public ProgressBar Progress;
        public ComboBox LotEdges;

        public NumericUpDown LeftYard;
        public NumericUpDown RightYard;
        public NumericUpDown FrontYard;
        public NumericUpDown BackYard;
        public NumericUpDown ClassValueChange;

        public CheckBox LeftRoad;
        public CheckBox RightRoad;
        public CheckBox FrontRoad;
        public CheckBox BackRoad;
        public CheckBox KeepStreet;
        public CheckBox MoveLot;
        public CheckBox ChangeRoads;
        public CheckBox AllowShrink;
        public CheckBox RemoveFurniture;
        public CheckBox Hidden;
        public CheckBox BeachLot;
        public CheckBox RemoveTerrainPaints;
        public CheckBox MatchHoodTerrain;
        public CheckBox LeavePortals;
        public CheckBox PaveRoads;
        public CheckBox BumpyRoads;
        public CheckBox KeepElevation;
        public CheckBox UpdateHoodTerrain;
        public CheckBox ClassOverride;
        public CheckBox MultiBackup;

        public PictureBox PictureBack;
        public PictureBox PictureForward;
        public PictureBox PictureLeft;
        public PictureBox PictureRight;
        public PictureBox PictureLogo;

        public TextBox AdvancedExpl;
        public TextBox Explanation;
        public TextBox LongExpl;
        public TextBox MoveReset;
        public TextBox SizeError;
        public TextBox SunLocation;

        public Label ClassValueDisplay;
        public Label HeightMax;
        public Label HeightNew;
        public Label HeightOld;
        public Label LabelBack;
        public Label LabelEdges;
        public Label LabelFront;
        public Label LabelLeft;
        public Label LabelMax;
        public Label LabelMoveBack;
        public Label LabelMoveLeft;
        public Label LabelNew;
        public Label LabelOld;
        public Label LabelRight;
        public Label LabelRoad;
        public Label LabelSize;
        public Label LabelWidth;
        public Label LabelX0;
        public Label LabelX1;
        public Label LabelX2;
        public Label MoveBack;
        public Label MoveLeft;
        public Label MoveResetLabel;
        public Label WidthMax;
        public Label WidthNew;
        public Label WidthOld;
        #endregion

        private ResourceManager _designerResources;

        private T Make<T>(string name) where T : Control, new()
        {
            var c = new T { Name = name };
            // ApplyResources: the Text, Visible and Enabled her resx holds.
            string text = _designerResources.GetString(name + ".Text", CultureInfo.InvariantCulture);
            if (text != null) c.Text = text;
            string visible = _designerResources.GetString(name + ".Visible", CultureInfo.InvariantCulture);
            if (visible != null) c.Visible = bool.Parse(visible);
            string enabled = _designerResources.GetString(name + ".Enabled", CultureInfo.InvariantCulture);
            if (enabled != null) c.Enabled = bool.Parse(enabled);
            return c;
        }

        private void InitializeComponent()
        {
            _designerResources = new ResourceManager("LotExpander.PrimaryForm", typeof(PrimaryForm).Assembly);
            Name = "PrimaryForm";
            Text = _designerResources.GetString("$this.Text", CultureInfo.InvariantCulture) ?? "LotAdjuster";

            Liste = Make<ListBox>("Liste");
            Title = Make<Label>("Title");
            NextButton = Make<Button>("NextButton");
            BackButton = Make<Button>("BackButton");
            AdvancedButton = Make<Button>("AdvancedButton");
            Defaults = Make<Button>("Defaults");
            LotProperties = Make<GroupBox>("LotProperties");
            AdvancedFeatures = Make<GroupBox>("AdvancedFeatures");
            ClassValuePanel = Make<Panel>("ClassValuePanel");
            Progress = Make<ProgressBar>("Progress");
            LotEdges = Make<ComboBox>("LotEdges");

            LeftYard = Make<NumericUpDown>("LeftYard");
            RightYard = Make<NumericUpDown>("RightYard");
            FrontYard = Make<NumericUpDown>("FrontYard");
            BackYard = Make<NumericUpDown>("BackYard");
            ClassValueChange = Make<NumericUpDown>("ClassValueChange");

            LeftRoad = Make<CheckBox>("LeftRoad");
            RightRoad = Make<CheckBox>("RightRoad");
            FrontRoad = Make<CheckBox>("FrontRoad");
            BackRoad = Make<CheckBox>("BackRoad");
            KeepStreet = Make<CheckBox>("KeepStreet");
            MoveLot = Make<CheckBox>("MoveLot");
            ChangeRoads = Make<CheckBox>("ChangeRoads");
            AllowShrink = Make<CheckBox>("AllowShrink");
            RemoveFurniture = Make<CheckBox>("RemoveFurniture");
            Hidden = Make<CheckBox>("Hidden");
            BeachLot = Make<CheckBox>("BeachLot");
            RemoveTerrainPaints = Make<CheckBox>("RemoveTerrainPaints");
            MatchHoodTerrain = Make<CheckBox>("MatchHoodTerrain");
            LeavePortals = Make<CheckBox>("LeavePortals");
            PaveRoads = Make<CheckBox>("PaveRoads");
            BumpyRoads = Make<CheckBox>("BumpyRoads");
            KeepElevation = Make<CheckBox>("KeepElevation");
            UpdateHoodTerrain = Make<CheckBox>("UpdateHoodTerrain");
            ClassOverride = Make<CheckBox>("ClassOverride");
            MultiBackup = Make<CheckBox>("MultiBackup");

            PictureBack = Make<PictureBox>("PictureBack");
            PictureForward = Make<PictureBox>("PictureForward");
            PictureLeft = Make<PictureBox>("PictureLeft");
            PictureRight = Make<PictureBox>("PictureRight");
            PictureLogo = Make<PictureBox>("PictureLogo");

            AdvancedExpl = Make<TextBox>("AdvancedExpl");
            Explanation = Make<TextBox>("Explanation");
            LongExpl = Make<TextBox>("LongExpl");
            MoveReset = Make<TextBox>("MoveReset");
            SizeError = Make<TextBox>("SizeError");
            SunLocation = Make<TextBox>("SunLocation");

            ClassValueDisplay = Make<Label>("ClassValueDisplay");
            HeightMax = Make<Label>("HeightMax");
            HeightNew = Make<Label>("HeightNew");
            HeightOld = Make<Label>("HeightOld");
            LabelBack = Make<Label>("LabelBack");
            LabelEdges = Make<Label>("LabelEdges");
            LabelFront = Make<Label>("LabelFront");
            LabelLeft = Make<Label>("LabelLeft");
            LabelMax = Make<Label>("LabelMax");
            LabelMoveBack = Make<Label>("LabelMoveBack");
            LabelMoveLeft = Make<Label>("LabelMoveLeft");
            LabelNew = Make<Label>("LabelNew");
            LabelOld = Make<Label>("LabelOld");
            LabelRight = Make<Label>("LabelRight");
            LabelRoad = Make<Label>("LabelRoad");
            LabelSize = Make<Label>("LabelSize");
            LabelWidth = Make<Label>("LabelWidth");
            LabelX0 = Make<Label>("LabelX0");
            LabelX1 = Make<Label>("LabelX1");
            LabelX2 = Make<Label>("LabelX2");
            MoveBack = Make<Label>("MoveBack");
            MoveLeft = Make<Label>("MoveLeft");
            MoveResetLabel = Make<Label>("MoveResetLabel");
            WidthMax = Make<Label>("WidthMax");
            WidthNew = Make<Label>("WidthNew");
            WidthOld = Make<Label>("WidthOld");

            // Values her Designer set in code rather than through the resx.
            LeftYard.Maximum = 60;  LeftYard.Minimum = -50;
            RightYard.Maximum = 60; RightYard.Minimum = -50;
            FrontYard.Maximum = 60; FrontYard.Minimum = -50;
            BackYard.Maximum = 60;  BackYard.Minimum = -50;
            SizeError.ReadOnly = true;
            Explanation.ReadOnly = true;
            MoveReset.ReadOnly = true;
            KeepElevation.Checked = true;
            LotEdges.Items.AddRange(new object[] {
                _designerResources.GetString("LotEdges.Items", CultureInfo.InvariantCulture),
                _designerResources.GetString("LotEdges.Items1", CultureInfo.InvariantCulture),
                _designerResources.GetString("LotEdges.Items2", CultureInfo.InvariantCulture)});
            ClassValueChange.Maximum = uint.MaxValue;
            ClassValueChange.Value = uint.MaxValue;
            AdvancedExpl.ReadOnly = true;
            LongExpl.ReadOnly = true;
            SunLocation.ReadOnly = true;

            // Event wiring, in her Designer's order.
            Liste.DoubleClick += new EventHandler(Liste_DoubleClick);
            Liste.SelectedIndexChanged += new EventHandler(Liste_SelectedIndexChanged);
            Liste.KeyDown += new KeyEventHandler(Liste_KeyDown);
            NextButton.Click += new EventHandler(NextButton_Click);
            BackButton.Click += new EventHandler(BackButton_Click);
            LeftRoad.Enter += new EventHandler(Road_Enter);
            LeftRoad.Leave += new EventHandler(Road_Leave);
            LeftRoad.CheckStateChanged += new EventHandler(LeftRoad_CheckStateChanged);
            RightRoad.Enter += new EventHandler(Road_Enter);
            RightRoad.Leave += new EventHandler(Road_Leave);
            RightRoad.CheckStateChanged += new EventHandler(RightRoad_CheckStateChanged);
            FrontRoad.Enter += new EventHandler(Road_Enter);
            FrontRoad.Leave += new EventHandler(Road_Leave);
            FrontRoad.CheckStateChanged += new EventHandler(FrontRoad_CheckStateChanged);
            BackRoad.Enter += new EventHandler(Road_Enter);
            BackRoad.Leave += new EventHandler(Road_Leave);
            BackRoad.CheckStateChanged += new EventHandler(BackRoad_CheckStateChanged);
            KeepStreet.Enter += new EventHandler(AdvancedMouseHover);
            KeepStreet.Leave += new EventHandler(AdvancedMouseLeave);
            KeepStreet.CheckedChanged += new EventHandler(KeepStreet_CheckedChanged);
            KeepStreet.MouseHover += new EventHandler(AdvancedMouseHover);
            AdvancedButton.Click += new EventHandler(AdvancedButton_Click);
            AdvancedFeatures.MouseHover += new EventHandler(AdvancedMouseLeave);
            MoveLot.Enter += new EventHandler(AdvancedMouseHover);
            MoveLot.Leave += new EventHandler(AdvancedMouseLeave);
            MoveLot.CheckedChanged += new EventHandler(MoveLot_CheckedChanged);
            MoveLot.MouseHover += new EventHandler(AdvancedMouseHover);
            LabelMoveLeft.Enter += new EventHandler(AdvancedMouseHover);
            MoveLeft.Enter += new EventHandler(AdvancedMouseHover);
            LabelMoveBack.Enter += new EventHandler(AdvancedMouseHover);
            MoveBack.Enter += new EventHandler(AdvancedMouseHover);
            PictureBack.Click += new EventHandler(PictureBack_Click);
            PictureLeft.Click += new EventHandler(PictureLeft_Click);
            MoveResetLabel.Enter += new EventHandler(MoveReset_Click);
            MoveResetLabel.Click += new EventHandler(MoveReset_Click);
            MoveReset.Enter += new EventHandler(MoveReset_Click);
            MoveReset.Click += new EventHandler(MoveReset_Click);
            PictureRight.Click += new EventHandler(PictureRight_Click);
            PictureForward.Click += new EventHandler(PictureForward_Click);
            ChangeRoads.Enter += new EventHandler(AdvancedMouseHover);
            ChangeRoads.Leave += new EventHandler(AdvancedMouseLeave);
            ChangeRoads.CheckedChanged += new EventHandler(ChangeRoads_CheckedChanged);
            ChangeRoads.MouseHover += new EventHandler(AdvancedMouseHover);
            AllowShrink.Enter += new EventHandler(AdvancedMouseHover);
            AllowShrink.Leave += new EventHandler(AdvancedMouseLeave);
            AllowShrink.MouseHover += new EventHandler(AdvancedMouseHover);
            AllowShrink.CheckStateChanged += new EventHandler(AllowShrink_CheckStateChanged);
            RemoveFurniture.Enter += new EventHandler(AdvancedMouseHover);
            RemoveFurniture.Leave += new EventHandler(AdvancedMouseLeave);
            RemoveFurniture.CheckedChanged += new EventHandler(AdvancedForeColor);
            RemoveFurniture.MouseHover += new EventHandler(AdvancedMouseHover);
            Hidden.Enter += new EventHandler(AdvancedMouseHover);
            Hidden.Leave += new EventHandler(AdvancedMouseLeave);
            Hidden.CheckedChanged += new EventHandler(AdvancedForeColor);
            Hidden.MouseHover += new EventHandler(AdvancedMouseHover);
            BeachLot.Enter += new EventHandler(AdvancedMouseHover);
            BeachLot.Leave += new EventHandler(AdvancedMouseLeave);
            BeachLot.MouseHover += new EventHandler(AdvancedMouseHover);
            RemoveTerrainPaints.Enter += new EventHandler(AdvancedMouseHover);
            RemoveTerrainPaints.Leave += new EventHandler(AdvancedMouseLeave);
            RemoveTerrainPaints.MouseHover += new EventHandler(AdvancedMouseHover);
            MatchHoodTerrain.Enter += new EventHandler(AdvancedMouseHover);
            MatchHoodTerrain.Leave += new EventHandler(AdvancedMouseLeave);
            MatchHoodTerrain.MouseHover += new EventHandler(AdvancedMouseHover);
            LeavePortals.Enter += new EventHandler(AdvancedMouseHover);
            LeavePortals.Leave += new EventHandler(AdvancedMouseLeave);
            LeavePortals.CheckedChanged += new EventHandler(AdvancedForeColor);
            LeavePortals.MouseHover += new EventHandler(AdvancedMouseHover);
            PaveRoads.Enter += new EventHandler(AdvancedMouseHover);
            PaveRoads.Leave += new EventHandler(AdvancedMouseLeave);
            PaveRoads.CheckedChanged += new EventHandler(PaveRoads_CheckedChanged);
            PaveRoads.MouseHover += new EventHandler(AdvancedMouseHover);
            BumpyRoads.Enter += new EventHandler(AdvancedMouseHover);
            BumpyRoads.Leave += new EventHandler(AdvancedMouseLeave);
            BumpyRoads.CheckedChanged += new EventHandler(AdvancedForeColor);
            BumpyRoads.MouseHover += new EventHandler(AdvancedMouseHover);
            KeepElevation.Enter += new EventHandler(AdvancedMouseHover);
            KeepElevation.Leave += new EventHandler(AdvancedMouseLeave);
            KeepElevation.MouseHover += new EventHandler(AdvancedMouseHover);
            UpdateHoodTerrain.Enter += new EventHandler(AdvancedMouseHover);
            UpdateHoodTerrain.Leave += new EventHandler(AdvancedMouseLeave);
            UpdateHoodTerrain.MouseHover += new EventHandler(AdvancedMouseHover);
            LabelEdges.Enter += new EventHandler(AdvancedMouseHover);
            LabelEdges.Leave += new EventHandler(AdvancedMouseLeave);
            LabelEdges.MouseHover += new EventHandler(AdvancedMouseHover);
            LotEdges.Enter += new EventHandler(AdvancedMouseHover);
            LotEdges.Leave += new EventHandler(AdvancedMouseLeave);
            LotEdges.MouseHover += new EventHandler(AdvancedMouseHover);
            LotEdges.SelectedIndexChanged += new EventHandler(AdvancedMouseHover);
            ClassOverride.Enter += new EventHandler(AdvancedMouseHover);
            ClassOverride.Leave += new EventHandler(AdvancedMouseLeave);
            ClassOverride.CheckedChanged += new EventHandler(ClassOverride_CheckedChanged);
            ClassOverride.MouseHover += new EventHandler(AdvancedMouseHover);
            ClassValueDisplay.Enter += new EventHandler(AdvancedMouseHover);
            ClassValueDisplay.Leave += new EventHandler(AdvancedMouseLeave);
            ClassValueDisplay.MouseHover += new EventHandler(AdvancedMouseHover);
            ClassValueChange.Enter += new EventHandler(AdvancedMouseHover);
            ClassValueChange.Leave += new EventHandler(AdvancedMouseLeave);
            ClassValueChange.MouseHover += new EventHandler(AdvancedMouseHover);
            MultiBackup.Enter += new EventHandler(AdvancedMouseHover);
            MultiBackup.Leave += new EventHandler(AdvancedMouseLeave);
            MultiBackup.CheckedChanged += new EventHandler(MultiBackup_CheckedChanged);
            MultiBackup.MouseHover += new EventHandler(AdvancedMouseHover);
            Defaults.Click += new EventHandler(Defaults_Click);
            Shown += new EventHandler(LotExpander_Shown);
            FormClosing += new FormClosingEventHandler(LotExpander_FormClosing);
            Load += new EventHandler(LotExpander_Load);
        }

        #region Host API (not in her form)
        // Her Neighborhood screen found hoods under
        // My Documents\EA Games\<registry DisplayName>\Neighborhoods. On macOS
        // the host finds them (SimsPaths / NeighborhoodCatalog) and hands the
        // chosen package here. This does what her neighborhood list did on
        // double-click: set NBPack, then show the Lot screen.
        public void OpenNeighborhood(string packagePath)
        {
            if (NBPack != null)
                NBPack.Close(true);
            NBPack = SimPe.Packages.File.LoadFromFile(packagePath);
            this.Tag = 0;
            Screen = Screen_Neighborhood;
            LotScreen();
        }

        public int CurrentScreen => Screen;
        public const int ScreenInitial = Screen_Initial;
        public const int ScreenNeighborhood = Screen_Neighborhood;
        public const int ScreenLot = Screen_Lot;
        public const int ScreenAdvanced = Screen_Advanced;
        public const int ScreenExpansion = Screen_Expansion;
        public const int ScreenFinal = Screen_Final;

        // The lot chosen on the Lot screen (null until Next is clicked there).
        public R_DESC SelectedLot => Lot;

        // The explanation her form showed on hover, per control.
        public string ExplanationFor(object control)
            => sExpl.TryGetValue(control, out var s) ? s : null;

        public string NeighborhoodFileName => NBPack?.FileName;
        #endregion
    }
}
