using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishbot
{
	public class MyProcess
	{
		public string ProcessName { get; set; }
        public int ProcessId { get; set; }

		public MyProcess(string name, int id)
		{
			this.ProcessName = name;
			this.ProcessId = id;
		}
    }
}
