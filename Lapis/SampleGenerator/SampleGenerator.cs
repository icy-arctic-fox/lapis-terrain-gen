using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lapis.Level.Data;
using Lapis.Level.Generation;

namespace SampleGenerator
{
    public class SampleGenerator : ITerrainGenerator
    {
	    public string GeneratorName
	    {
			get { return "Sample Generator"; }
	    }

	    public int GeneratorVersion
	    {
			get { return 1; }
	    }

	    public string GeneratorOptions
	    {
			get { return String.Empty; }
	    }

	    public void Initialize (string options)
	    {
		    throw new NotImplementedException();
	    }

	    public ChunkData GenerateChunk (int cx, int cz)
	    {
		    throw new NotImplementedException();
	    }
    }
}
