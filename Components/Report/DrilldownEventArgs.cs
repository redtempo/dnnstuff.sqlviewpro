

using System;
using System.Data;



//***************************************************************************/
//* DrilldownEventArgs.vb
//*
//* COPYRIGHT (c) 2004-2011 by DNNStuff
//* ALL RIGHTS RESERVED.
//*
//* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//* DEALINGS IN THE SOFTWARE.
//*************/

namespace DNNStuff.SQLViewPro.Controls
{
	public class DrilldownEventArgs : EventArgs
	{
		
		public DrilldownEventArgs(int reportId, string name, DataRow value)
		{
			_reportId = reportId;
			_name = name;
			_value = value;
		}
		
#region Properties
		private int _reportId;
		
		public int ReportId
		{
			get
			{
				return _reportId;
			}
			set
			{
				_reportId = value;
			}
		}
		
		private DataRow _value;
		public DataRow Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
		
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
#endregion
		
	}
}

