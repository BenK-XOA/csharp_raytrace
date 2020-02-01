using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace RayTracer
{
   public partial class Raytracer : Form
    {
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem dateiToolStripMenuItem;
        private ToolStripMenuItem renderToolStripMenuItem;
        private ToolStripMenuItem fBXToolStripMenuItem;
        private PictureBox pictureBox1;

        public Raytracer()
        {
            InitializeComponent();
            render();
        }

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Raytracer());
            
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fBXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.pictureBox1.Location = new System.Drawing.Point(1, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1920, 1080);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1904, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.renderToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fBXToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dateiToolStripMenuItem.Text = "Load Object";
            // 
            // renderToolStripMenuItem
            // 
            this.renderToolStripMenuItem.Name = "renderToolStripMenuItem";
            this.renderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renderToolStripMenuItem.Text = "Render";
            // 
            // fBXToolStripMenuItem
            // 
            this.fBXToolStripMenuItem.Name = "fBXToolStripMenuItem";
            this.fBXToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fBXToolStripMenuItem.Text = "FBX";
            this.fBXToolStripMenuItem.Click += new System.EventHandler(this.fBXToolStripMenuItem_Click);
            // 
            // Program
            // 
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Program";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void render()
        {
            
            Bitmap rendering = new Bitmap(1920, 1080);
            Sphere[] spheres = new Sphere[]
            {
                new Sphere(new Vector3(0, -1 , 3), 1,  new Color(255, 0, 0), 500, 0.06f),
                new Sphere(new Vector3(-2, 1 , 3), 1,  new Color(0, 0, 255), 500, 0.1f),
                new Sphere(new Vector3(2, 1 , 3), 1,  new Color(0, 255, 0), 10, 0),
                new Sphere(new Vector3(0, -5001 , 0), 5000,  new Color(255, 255, 0), 10, 0f)
            };
            Light[] lights = new Light[]
            {
                new Light(LightType.ambient,0.2f, new Vector3(0,0,0), new Vector3(0,0,0)),
                new Light(LightType.point,0.6f, new Vector3(2,1,0), new Vector3(0,0,0)),
                new Light(LightType.directional,0.2f, new Vector3(1,4,4), new Vector3(0,0,0))
            };

            Scene scene = new Scene(spheres, lights);
            Camera cam = new Camera(new Vector3(0, 0, -5));

            Raytracer tracer = new Raytracer(cam, new Viewport((float)rendering.Width / (float)rendering.Height, 1, 1), rendering, scene, new Color(255, 255, 255));

            Program tracer = new Program(new Viewport(2, 2, 1), rendering, scene, new Color(255, 255, 255));

            tracer.Render();
            rendering.RotateFlip(RotateFlipType.Rotate180FlipNone);
            this.pictureBox1.Image = rendering;

            //for mac development
            rendering.Save("renderresult.png");

        }

        private void fBXToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
