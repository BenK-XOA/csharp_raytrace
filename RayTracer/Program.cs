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
   public partial class Program : Form
    {
        private PictureBox pictureBox1;

        public Program()
        {
            InitializeComponent();
            render();
        }

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
            
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            // Program
            // 
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Program";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

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


            Program tracer = new Program(new Viewport(2, 2, 1), rendering, scene, new Color(255, 255, 255));

            tracer.Render();
            rendering.RotateFlip(RotateFlipType.Rotate180FlipNone);
            this.pictureBox1.Image = rendering;
        }
    }
}
