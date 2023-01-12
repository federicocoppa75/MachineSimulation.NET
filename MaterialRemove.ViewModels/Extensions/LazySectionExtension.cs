using MaterialRemove.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Extensions
{
    internal static class LazySectionExtension
    {
        /// <summary>
        /// Metodo che restituisce la posizione della sottosezione (i,j) in una sezione lazy del pannello
        /// </summary>
        /// <param name="sectionPosition">posizionamento della sezione lazy da sezionare</param>
        /// <param name="nxSection">numero delle divisioni lungo X</param>
        /// <param name="nySection">numero delle divisioni lungo X</param>
        /// <param name="i">indice posizione X</param>
        /// <param name="j">indice posizione Y</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">viene restituita quando non si riesce a restituire un risultato valido con gli argomenti passati</exception>
        internal static SectionPosition GetSectionPosition(SectionPosition sectionPosition, int nxSection, int nySection, int i, int j)
        {
            var result = SectionPosition.Center;

            switch (sectionPosition)
            {
                case SectionPosition.Center:
                    result = SectionPosition.Center;
                    break;
                case SectionPosition.SideTop:
                    result = (j == nySection - 1) ? SectionPosition.SideTop : SectionPosition.Center;
                    break;
                case SectionPosition.SideRigth:
                    result = (i == nxSection - 1) ? SectionPosition.SideRigth : SectionPosition.Center;
                    break;
                case SectionPosition.SideBottom:
                    result = (j == 0) ? SectionPosition.SideBottom : SectionPosition.Center;
                    break;
                case SectionPosition.SideLeft:
                    result = (i == 0) ? SectionPosition.SideLeft : SectionPosition.Center;
                    break;
                case SectionPosition.CornerTopRight:
                    result = GetSectionPosition(() => (j == nySection - 1), () => (i == nxSection - 1), SectionPosition.SideTop, SectionPosition.SideRigth);
                    break;
                case SectionPosition.CornerTopLeft:
                    result = GetSectionPosition(() => (j == nySection - 1), () => (i == 0), SectionPosition.SideTop, SectionPosition.SideLeft);
                    break;
                case SectionPosition.CornerBottomLeft:
                    result = GetSectionPosition(() => (j == 0), () => (i == 0), SectionPosition.SideBottom, SectionPosition.SideLeft);
                    break;
                case SectionPosition.CornerBottomRight:
                    result = GetSectionPosition(() => (j == 0), () => (i == nxSection - 1), SectionPosition.SideBottom, SectionPosition.SideRigth);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        static SectionPosition GetSectionPosition(Func<bool> funcIsOnSide1, Func<bool> funcIsOnSide2, SectionPosition side1, SectionPosition side2)
        {
            var isOnSide1 = funcIsOnSide1();
            var isOnSide2 = funcIsOnSide2();

            if (isOnSide1 && isOnSide2)
            {
                return GetCorner(side1, side2);
            }
            else if (isOnSide1 && !isOnSide2)
            {
                return side1;
            }
            else if (!isOnSide1 && isOnSide2)
            {
                return side2;
            }
            else
            {
                return SectionPosition.Center;
            }
        }

        static SectionPosition GetCorner(SectionPosition side1, SectionPosition side2)
        {
            if (IsCornerBomomLeft(side1, side2)) return SectionPosition.CornerBottomLeft;
            else if (IsCornerBomomRight(side1, side2)) return SectionPosition.CornerBottomRight;
            else if (IsCornerTopLeft(side1, side2)) return SectionPosition.CornerTopLeft;
            else if (IsCornerTopRight(side1, side2)) return SectionPosition.CornerTopRight;
            else throw new ArgumentException($"No corner condition with {side1} and {side2}");
        }

        static bool IsCornerBomomLeft(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideBottom, SectionPosition.SideLeft);
        static bool IsCornerBomomRight(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideBottom, SectionPosition.SideRigth);
        static bool IsCornerTopLeft(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideTop, SectionPosition.SideLeft);
        static bool IsCornerTopRight(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideTop, SectionPosition.SideRigth);


        static bool IsCorner(SectionPosition side1, SectionPosition side2, SectionPosition request1, SectionPosition request2)
        {
            return ((side1 == request1) && (side2 == request2)) ||
                    ((side1 == request2) && (side2 == request1));
        }
    }
}
