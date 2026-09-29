/***************************************************************************
 *   LotAdjuster © 2008–2013 Mootilda                                      *
 *   macOS port © 2026 GramzeSweatshop                                     *
 *   GNU GPLv2 or later, see LICENSE.                                      *
 ***************************************************************************/
// Mootilda's R_POOL/R_VERT/R_3ARY_* handlers call two static helpers that live
// on her WinForms form (LotExpander.cs lines 130-142). The form itself does not
// come across to macOS, so this partial carries those two methods VERBATIM so
// the handlers compile unchanged. Her lot-adjust logic (FinalScreen and
// friends) will be lifted into further partials of this class.

using System.Resources;

namespace LotExpander
{
    public partial class PrimaryForm
    {
        public static string ThrowErrorOffLot(string sType, string sList)
        {
            ResourceManager RME = new ResourceManager("LotExpander.LEStrings", typeof(PrimaryForm).Assembly);
            throw new ShrinkException(
                string.Format(RME.GetString("ExplainShrinkAbort"), sType, sList));
        }

        public static string ThrowErrorEmptyLot(string sLotName)
        {
            ResourceManager RME = new ResourceManager("LotExpander.LEStrings", typeof(PrimaryForm).Assembly);
            throw new EmptyLotException(
                string.Format(RME.GetString("ExplainEmptyLotAbort"), sLotName));
        }
    }
}
