using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEditor.Model
{
    //This class contains arrays of numbers from Gaussian(normal distribution)

    public class GaussianMatrixForBlur
    {
        public static double[,] GaussianBlur3x3
        {
            get
            {
                return new double[,]
                { {  (double)1/16, (double)2/16, (double)1/16, },
                  {  (double)2/16, (double)4/16, (double)2/16, },
                  { (double)1/16, (double)2/16, (double)1/16, }, };
            }
        }


        public static double[,] GaussianBlur5x5
        {
            get
            {
                return new double[,]
                { {  (double)2/159, (double)04/159, (double)05/159, (double)04/159, (double)2/159 },
                  {  (double)4/159, (double)09/159, (double)12/159, (double)09/159, (double)4/159 },
                  {  (double)5/159, (double)12/159, (double)15/159, (double)12/159, (double)5/159 },
                  {  (double)4/159, (double)09/159, (double)12/159, (double)09/159, (double)4/159 },
                  {  (double)2/159, (double)04/159, (double)05/159, (double)04/159, (double)2/159 }, };
            }
        }
    }
}
