using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xml解析
{
    class shpfile
    {
        public string ModelName { get; set; }
        public string LocationX { get; set; }
        public string LocationY { get; set; }
        public string LocationZ { get; set; }
        public string Matrix3 { get; set; }

        public override string ToString()
        {
            return string.Format("ModelName: {0}, LocationX: {1}, LocationY: {2}, LocationZ: {3}，Matrix3：{4}", ModelName, LocationX, LocationY, LocationZ, Matrix3);
        }

    }
}
