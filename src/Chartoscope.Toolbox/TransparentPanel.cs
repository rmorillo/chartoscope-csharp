using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chartoscope.Toolbox
{
    public partial class TransparentPanel : Panel

  {

 

    Timer Wriggler=new Timer();

 

    public TransparentPanel()

    {

      //

      // TODO: Add constructor logic here

      //

 

      Wriggler.Tick+=new EventHandler(TickHandler);

      this.Wriggler.Interval=500;

      //this.Wriggler.Enabled=true;

    }

 

    protected void TickHandler(object sender, EventArgs e)

    {

      this.InvalidateEx();

    } 

 

    protected override CreateParams CreateParams

    {

      get

      {

        CreateParams cp=base.CreateParams;

        cp.ExStyle|=0x00000020; //WS_EX_TRANSPARENT

        return cp;

      }

    }

 

    protected void InvalidateEx()

    {

      if(Parent==null)

        return;

 

      Rectangle rc=new Rectangle(this.Location,this.Size);

      Parent.Invalidate(rc,true);

    }

 

    protected override void OnPaintBackground(PaintEventArgs pevent)

    {

      //do not allow the background to be painted 

    }

 

    Random r=new Random();

 

    protected override void OnPaint(PaintEventArgs e)

    {

            int h=this.Height/2;

      int w=this.Width/2;

 

      Pen p=new Pen(Color.DarkGray,1);

      int x,y;

      //for(x=0,y=0; x<w; x+=w/10, y+=h/10)

      //{

      //  e.Graphics.DrawEllipse(p,x+r.Next(10)-5,y+r.Next(10)-5,this.Width-(2*x),this.Height-(2*y));

      //}

      e.Graphics.DrawLine(p, new Point(3, 0), new Point(3, this.Height));
      e.Graphics.DrawLine(p, new Point(7, 0), new Point(7, this.Height));

      p.Dispose();

    }

 

 

  }

}
