using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeightmapTerrainStarter
{
    /// <summary>
    /// An interface for determining the height at a point in a height map
    /// </summary>
    public interface IHeightMap
    {
        float GetHeightAt(float x, float z);
    }
}
