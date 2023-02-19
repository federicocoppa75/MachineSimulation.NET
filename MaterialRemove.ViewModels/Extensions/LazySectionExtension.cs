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
                case SectionPosition.CenterAlongX:
                    result = GetSectionPosition(() => (j == 0), () => (j == nySection - 1), SectionPosition.SideBottom, SectionPosition.SideTop);
                    break; 
                case SectionPosition.CenterAlongY:
                    result = GetSectionPosition(() => (i == 0), () => (i == nxSection - 1), SectionPosition.SideLeft, SectionPosition.SideRigth);
                    break;
                case SectionPosition.EndBottom:
                    result = GetSectionPosition(() => (i == 0), () => (i == nxSection-1), () => (j == 0), SectionPosition.SideLeft, SectionPosition.SideRigth, SectionPosition.SideBottom);
                    break;
                case SectionPosition.EndLeft:
                    result = GetSectionPosition(() => (i == 0), () => (j == 0), () => (j == nySection - 1), SectionPosition.SideLeft, SectionPosition.SideBottom, SectionPosition.SideTop);
                    break;
                case SectionPosition.EndRight:
                    result = GetSectionPosition(() => (j == 0), () => (j == nySection - 1), () => (i == 0), SectionPosition.SideBottom, SectionPosition.SideTop, SectionPosition.SideRigth);
                    break;
                case SectionPosition.EndTop:
                    result = GetSectionPosition(() => (i == 0), () => (i == nxSection - 1), () => (j == nySection - 1), SectionPosition.SideLeft, SectionPosition.SideRigth, SectionPosition.SideTop);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }


        internal static SectionPosition GetSectionPositionX(int nSection, int i)
        {
            if (i == 0)
            {
                return SectionPosition.EndLeft;
            }
            else if (i == nSection - 1)
            {
                return SectionPosition.EndRight;
            }
            else
            {
                return SectionPosition.CenterAlongX;
            }
        }
        internal static SectionPosition GetSectionPositionY(int nSection, int i)
        {
            if (i == 0)
            {
                return SectionPosition.EndBottom;
            }
            else if (i == nSection - 1)
            {
                return SectionPosition.EndTop;
            }
            else
            {
                return SectionPosition.CenterAlongY;
            }
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

        static SectionPosition GetSectionPosition(Func<bool> funcIsOnSide1, Func<bool> funcIsOnSide2, Func<bool> funcIsOnSide3, SectionPosition side1, SectionPosition side2, SectionPosition side3)
        {
            var isOnSides = new bool[] 
            {
                funcIsOnSide1(),
                funcIsOnSide2(),
                funcIsOnSide3()
            };

            var sides = new SectionPosition[]
            {
                side1,
                side2,
                side3
            };

            var isOnSide1 = funcIsOnSide1();
            var isOnSide2 = funcIsOnSide2();
            var isOnSide3 = funcIsOnSide3();

            return GetSectionPosition(isOnSides, sides);
        }

        static SectionPosition GetSectionPosition(bool[] isOnSides, SectionPosition[] sides)
        {
            var nCheck = 0;
            var sCheck = new SectionPosition[sides.Length];

            for (int i = 0; i < isOnSides.Length; i++)
            {
                if (isOnSides[i])
                {
                    sCheck[nCheck] = sides[i];
                    nCheck++;
                }
            }

            if(nCheck == 0) 
            {
                return SectionPosition.Center;
            }
            else if(nCheck == 1)
            {
                return sCheck[0];
            }
            else if(nCheck == 2)
            {
                return GetCorner(sCheck[0], sCheck[1]);
            }
            else
            {
                throw new NotImplementedException($"Side condition with {nCheck} sides not implemented!");
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
