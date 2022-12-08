namespace SteelStack
{
    partial class inputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_originX = new System.Windows.Forms.Label();
            this.txt_originX = new System.Windows.Forms.TextBox();
            this.lbl_originY = new System.Windows.Forms.Label();
            this.txt_originY = new System.Windows.Forms.TextBox();
            this.lbl_originZ = new System.Windows.Forms.Label();
            this.txt_originZ = new System.Windows.Forms.TextBox();
            this.btn_createModel = new System.Windows.Forms.Button();
            this.txt_topDiameter1 = new System.Windows.Forms.TextBox();
            this.lbl_topDiameter = new System.Windows.Forms.Label();
            this.lbl_thickness = new System.Windows.Forms.Label();
            this.txt_thickness1 = new System.Windows.Forms.TextBox();
            this.txt_bottomDiameter1 = new System.Windows.Forms.TextBox();
            this.txt_height1 = new System.Windows.Forms.TextBox();
            this.lbl_bottomDiameter = new System.Windows.Forms.Label();
            this.lbl_height = new System.Windows.Forms.Label();
            this.txt_topDiameter2 = new System.Windows.Forms.TextBox();
            this.txt_topDiameter3 = new System.Windows.Forms.TextBox();
            this.txt_bottomDiameter2 = new System.Windows.Forms.TextBox();
            this.txt_bottomDiameter3 = new System.Windows.Forms.TextBox();
            this.txt_height2 = new System.Windows.Forms.TextBox();
            this.txt_height3 = new System.Windows.Forms.TextBox();
            this.txt_thickness2 = new System.Windows.Forms.TextBox();
            this.txt_thickness3 = new System.Windows.Forms.TextBox();
            this.lbl_stackSegments = new System.Windows.Forms.Label();
            this.lbl_seg1 = new System.Windows.Forms.Label();
            this.lbl_seg2 = new System.Windows.Forms.Label();
            this.lbl_seg3 = new System.Windows.Forms.Label();
            this.lbl_startAngle = new System.Windows.Forms.Label();
            this.lbl_endAngle = new System.Windows.Forms.Label();
            this.lbl_length = new System.Windows.Forms.Label();
            this.lbl_platform = new System.Windows.Forms.Label();
            this.lbl_platformExtension = new System.Windows.Forms.Label();
            this.txt_platformStartAngle = new System.Windows.Forms.TextBox();
            this.txt_platformEndAngle = new System.Windows.Forms.TextBox();
            this.txt_platformLength = new System.Windows.Forms.TextBox();
            this.txt_extensionStartAngle = new System.Windows.Forms.TextBox();
            this.txt_extensionEndAngle = new System.Windows.Forms.TextBox();
            this.txt_extensionLength = new System.Windows.Forms.TextBox();
            this.chk_handrail = new System.Windows.Forms.CheckBox();
            this.chk_platform = new System.Windows.Forms.CheckBox();
            this.chk_floorSteel = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbl_originX
            // 
            this.lbl_originX.AutoSize = true;
            this.lbl_originX.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_originX.Location = new System.Drawing.Point(25, 27);
            this.lbl_originX.Name = "lbl_originX";
            this.lbl_originX.Size = new System.Drawing.Size(85, 24);
            this.lbl_originX.TabIndex = 0;
            this.lbl_originX.Text = "Origin X";
            // 
            // txt_originX
            // 
            this.txt_originX.AccessibleName = "";
            this.txt_originX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_originX.Location = new System.Drawing.Point(116, 23);
            this.txt_originX.Name = "txt_originX";
            this.txt_originX.Size = new System.Drawing.Size(121, 31);
            this.txt_originX.TabIndex = 1;
            // 
            // lbl_originY
            // 
            this.lbl_originY.AutoSize = true;
            this.lbl_originY.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_originY.Location = new System.Drawing.Point(311, 27);
            this.lbl_originY.Name = "lbl_originY";
            this.lbl_originY.Size = new System.Drawing.Size(85, 24);
            this.lbl_originY.TabIndex = 2;
            this.lbl_originY.Text = "Origin Y";
            // 
            // txt_originY
            // 
            this.txt_originY.AccessibleName = "";
            this.txt_originY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_originY.Location = new System.Drawing.Point(402, 23);
            this.txt_originY.Name = "txt_originY";
            this.txt_originY.Size = new System.Drawing.Size(121, 31);
            this.txt_originY.TabIndex = 3;
            // 
            // lbl_originZ
            // 
            this.lbl_originZ.AutoSize = true;
            this.lbl_originZ.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_originZ.Location = new System.Drawing.Point(609, 27);
            this.lbl_originZ.Name = "lbl_originZ";
            this.lbl_originZ.Size = new System.Drawing.Size(84, 24);
            this.lbl_originZ.TabIndex = 4;
            this.lbl_originZ.Text = "Origin Z";
            this.lbl_originZ.Click += new System.EventHandler(this.txt_originZ_Click);
            // 
            // txt_originZ
            // 
            this.txt_originZ.AccessibleName = "";
            this.txt_originZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_originZ.Location = new System.Drawing.Point(700, 23);
            this.txt_originZ.Name = "txt_originZ";
            this.txt_originZ.Size = new System.Drawing.Size(121, 31);
            this.txt_originZ.TabIndex = 5;
            // 
            // btn_createModel
            // 
            this.btn_createModel.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_createModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_createModel.Location = new System.Drawing.Point(1030, 598);
            this.btn_createModel.Name = "btn_createModel";
            this.btn_createModel.Size = new System.Drawing.Size(140, 51);
            this.btn_createModel.TabIndex = 6;
            this.btn_createModel.Text = "Create Model";
            this.btn_createModel.UseVisualStyleBackColor = false;
            this.btn_createModel.Click += new System.EventHandler(this.btn_createModel_Click);
            // 
            // txt_topDiameter1
            // 
            this.txt_topDiameter1.AccessibleName = "";
            this.txt_topDiameter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_topDiameter1.Location = new System.Drawing.Point(130, 136);
            this.txt_topDiameter1.Name = "txt_topDiameter1";
            this.txt_topDiameter1.Size = new System.Drawing.Size(159, 31);
            this.txt_topDiameter1.TabIndex = 7;
            // 
            // lbl_topDiameter
            // 
            this.lbl_topDiameter.AutoSize = true;
            this.lbl_topDiameter.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_topDiameter.Location = new System.Drawing.Point(142, 109);
            this.lbl_topDiameter.Name = "lbl_topDiameter";
            this.lbl_topDiameter.Size = new System.Drawing.Size(134, 24);
            this.lbl_topDiameter.TabIndex = 8;
            this.lbl_topDiameter.Text = "Top Diameter";
            // 
            // lbl_thickness
            // 
            this.lbl_thickness.AutoSize = true;
            this.lbl_thickness.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_thickness.Location = new System.Drawing.Point(717, 109);
            this.lbl_thickness.Name = "lbl_thickness";
            this.lbl_thickness.Size = new System.Drawing.Size(104, 24);
            this.lbl_thickness.TabIndex = 11;
            this.lbl_thickness.Text = "Thickness";
            // 
            // txt_thickness1
            // 
            this.txt_thickness1.AccessibleName = "";
            this.txt_thickness1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_thickness1.Location = new System.Drawing.Point(688, 136);
            this.txt_thickness1.Name = "txt_thickness1";
            this.txt_thickness1.Size = new System.Drawing.Size(159, 31);
            this.txt_thickness1.TabIndex = 14;
            // 
            // txt_bottomDiameter1
            // 
            this.txt_bottomDiameter1.AccessibleName = "";
            this.txt_bottomDiameter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_bottomDiameter1.Location = new System.Drawing.Point(295, 136);
            this.txt_bottomDiameter1.Name = "txt_bottomDiameter1";
            this.txt_bottomDiameter1.Size = new System.Drawing.Size(197, 31);
            this.txt_bottomDiameter1.TabIndex = 15;
            // 
            // txt_height1
            // 
            this.txt_height1.AccessibleName = "";
            this.txt_height1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_height1.Location = new System.Drawing.Point(498, 136);
            this.txt_height1.Name = "txt_height1";
            this.txt_height1.Size = new System.Drawing.Size(184, 31);
            this.txt_height1.TabIndex = 16;
            // 
            // lbl_bottomDiameter
            // 
            this.lbl_bottomDiameter.AutoSize = true;
            this.lbl_bottomDiameter.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_bottomDiameter.Location = new System.Drawing.Point(311, 109);
            this.lbl_bottomDiameter.Name = "lbl_bottomDiameter";
            this.lbl_bottomDiameter.Size = new System.Drawing.Size(167, 24);
            this.lbl_bottomDiameter.TabIndex = 17;
            this.lbl_bottomDiameter.Text = "Bottom Diameter";
            // 
            // lbl_height
            // 
            this.lbl_height.AutoSize = true;
            this.lbl_height.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_height.Location = new System.Drawing.Point(547, 109);
            this.lbl_height.Name = "lbl_height";
            this.lbl_height.Size = new System.Drawing.Size(69, 24);
            this.lbl_height.TabIndex = 18;
            this.lbl_height.Text = "Height";
            // 
            // txt_topDiameter2
            // 
            this.txt_topDiameter2.AccessibleName = "";
            this.txt_topDiameter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_topDiameter2.Location = new System.Drawing.Point(130, 173);
            this.txt_topDiameter2.Name = "txt_topDiameter2";
            this.txt_topDiameter2.Size = new System.Drawing.Size(159, 31);
            this.txt_topDiameter2.TabIndex = 19;
            // 
            // txt_topDiameter3
            // 
            this.txt_topDiameter3.AccessibleName = "";
            this.txt_topDiameter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_topDiameter3.Location = new System.Drawing.Point(130, 210);
            this.txt_topDiameter3.Name = "txt_topDiameter3";
            this.txt_topDiameter3.Size = new System.Drawing.Size(159, 31);
            this.txt_topDiameter3.TabIndex = 20;
            // 
            // txt_bottomDiameter2
            // 
            this.txt_bottomDiameter2.AccessibleName = "";
            this.txt_bottomDiameter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_bottomDiameter2.Location = new System.Drawing.Point(295, 173);
            this.txt_bottomDiameter2.Name = "txt_bottomDiameter2";
            this.txt_bottomDiameter2.Size = new System.Drawing.Size(197, 31);
            this.txt_bottomDiameter2.TabIndex = 21;
            // 
            // txt_bottomDiameter3
            // 
            this.txt_bottomDiameter3.AccessibleName = "";
            this.txt_bottomDiameter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_bottomDiameter3.Location = new System.Drawing.Point(295, 210);
            this.txt_bottomDiameter3.Name = "txt_bottomDiameter3";
            this.txt_bottomDiameter3.Size = new System.Drawing.Size(197, 31);
            this.txt_bottomDiameter3.TabIndex = 22;
            // 
            // txt_height2
            // 
            this.txt_height2.AccessibleName = "";
            this.txt_height2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_height2.Location = new System.Drawing.Point(498, 173);
            this.txt_height2.Name = "txt_height2";
            this.txt_height2.Size = new System.Drawing.Size(184, 31);
            this.txt_height2.TabIndex = 23;
            // 
            // txt_height3
            // 
            this.txt_height3.AccessibleName = "";
            this.txt_height3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_height3.Location = new System.Drawing.Point(498, 210);
            this.txt_height3.Name = "txt_height3";
            this.txt_height3.Size = new System.Drawing.Size(184, 31);
            this.txt_height3.TabIndex = 24;
            // 
            // txt_thickness2
            // 
            this.txt_thickness2.AccessibleName = "";
            this.txt_thickness2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_thickness2.Location = new System.Drawing.Point(688, 173);
            this.txt_thickness2.Name = "txt_thickness2";
            this.txt_thickness2.Size = new System.Drawing.Size(159, 31);
            this.txt_thickness2.TabIndex = 25;
            // 
            // txt_thickness3
            // 
            this.txt_thickness3.AccessibleName = "";
            this.txt_thickness3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_thickness3.Location = new System.Drawing.Point(688, 210);
            this.txt_thickness3.Name = "txt_thickness3";
            this.txt_thickness3.Size = new System.Drawing.Size(159, 31);
            this.txt_thickness3.TabIndex = 26;
            // 
            // lbl_stackSegments
            // 
            this.lbl_stackSegments.AutoSize = true;
            this.lbl_stackSegments.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_stackSegments.Location = new System.Drawing.Point(404, 67);
            this.lbl_stackSegments.Name = "lbl_stackSegments";
            this.lbl_stackSegments.Size = new System.Drawing.Size(212, 31);
            this.lbl_stackSegments.TabIndex = 27;
            this.lbl_stackSegments.Text = "Stack Segments";
            // 
            // lbl_seg1
            // 
            this.lbl_seg1.AutoSize = true;
            this.lbl_seg1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_seg1.Location = new System.Drawing.Point(97, 139);
            this.lbl_seg1.Name = "lbl_seg1";
            this.lbl_seg1.Size = new System.Drawing.Size(24, 25);
            this.lbl_seg1.TabIndex = 28;
            this.lbl_seg1.Text = "1";
            // 
            // lbl_seg2
            // 
            this.lbl_seg2.AutoSize = true;
            this.lbl_seg2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_seg2.Location = new System.Drawing.Point(97, 176);
            this.lbl_seg2.Name = "lbl_seg2";
            this.lbl_seg2.Size = new System.Drawing.Size(24, 25);
            this.lbl_seg2.TabIndex = 29;
            this.lbl_seg2.Text = "2";
            // 
            // lbl_seg3
            // 
            this.lbl_seg3.AutoSize = true;
            this.lbl_seg3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_seg3.Location = new System.Drawing.Point(97, 213);
            this.lbl_seg3.Name = "lbl_seg3";
            this.lbl_seg3.Size = new System.Drawing.Size(24, 25);
            this.lbl_seg3.TabIndex = 30;
            this.lbl_seg3.Text = "3";
            // 
            // lbl_startAngle
            // 
            this.lbl_startAngle.AutoSize = true;
            this.lbl_startAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_startAngle.Location = new System.Drawing.Point(239, 281);
            this.lbl_startAngle.Name = "lbl_startAngle";
            this.lbl_startAngle.Size = new System.Drawing.Size(89, 20);
            this.lbl_startAngle.TabIndex = 31;
            this.lbl_startAngle.Text = "Start Angle";
            this.lbl_startAngle.Click += new System.EventHandler(this.lbl_startAngle_Click);
            // 
            // lbl_endAngle
            // 
            this.lbl_endAngle.AutoSize = true;
            this.lbl_endAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_endAngle.Location = new System.Drawing.Point(424, 281);
            this.lbl_endAngle.Name = "lbl_endAngle";
            this.lbl_endAngle.Size = new System.Drawing.Size(83, 20);
            this.lbl_endAngle.TabIndex = 32;
            this.lbl_endAngle.Text = "End Angle";
            // 
            // lbl_length
            // 
            this.lbl_length.AutoSize = true;
            this.lbl_length.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_length.Location = new System.Drawing.Point(609, 281);
            this.lbl_length.Name = "lbl_length";
            this.lbl_length.Size = new System.Drawing.Size(59, 20);
            this.lbl_length.TabIndex = 33;
            this.lbl_length.Text = "Length";
            // 
            // lbl_platform
            // 
            this.lbl_platform.AutoSize = true;
            this.lbl_platform.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_platform.Location = new System.Drawing.Point(25, 326);
            this.lbl_platform.Name = "lbl_platform";
            this.lbl_platform.Size = new System.Drawing.Size(68, 20);
            this.lbl_platform.TabIndex = 34;
            this.lbl_platform.Text = "Platform";
            // 
            // lbl_platformExtension
            // 
            this.lbl_platformExtension.AutoSize = true;
            this.lbl_platformExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_platformExtension.Location = new System.Drawing.Point(25, 406);
            this.lbl_platformExtension.Name = "lbl_platformExtension";
            this.lbl_platformExtension.Size = new System.Drawing.Size(142, 20);
            this.lbl_platformExtension.TabIndex = 35;
            this.lbl_platformExtension.Text = "Platform Extension";
            // 
            // txt_platformStartAngle
            // 
            this.txt_platformStartAngle.AccessibleName = "";
            this.txt_platformStartAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_platformStartAngle.Location = new System.Drawing.Point(220, 315);
            this.txt_platformStartAngle.Name = "txt_platformStartAngle";
            this.txt_platformStartAngle.Size = new System.Drawing.Size(121, 31);
            this.txt_platformStartAngle.TabIndex = 36;
            // 
            // txt_platformEndAngle
            // 
            this.txt_platformEndAngle.AccessibleName = "";
            this.txt_platformEndAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_platformEndAngle.Location = new System.Drawing.Point(402, 315);
            this.txt_platformEndAngle.Name = "txt_platformEndAngle";
            this.txt_platformEndAngle.Size = new System.Drawing.Size(121, 31);
            this.txt_platformEndAngle.TabIndex = 37;
            // 
            // txt_platformLength
            // 
            this.txt_platformLength.AccessibleName = "";
            this.txt_platformLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_platformLength.Location = new System.Drawing.Point(572, 315);
            this.txt_platformLength.Name = "txt_platformLength";
            this.txt_platformLength.Size = new System.Drawing.Size(121, 31);
            this.txt_platformLength.TabIndex = 38;
            // 
            // txt_extensionStartAngle
            // 
            this.txt_extensionStartAngle.AccessibleName = "";
            this.txt_extensionStartAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_extensionStartAngle.Location = new System.Drawing.Point(220, 395);
            this.txt_extensionStartAngle.Name = "txt_extensionStartAngle";
            this.txt_extensionStartAngle.Size = new System.Drawing.Size(121, 31);
            this.txt_extensionStartAngle.TabIndex = 39;
            // 
            // txt_extensionEndAngle
            // 
            this.txt_extensionEndAngle.AccessibleName = "";
            this.txt_extensionEndAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_extensionEndAngle.Location = new System.Drawing.Point(402, 395);
            this.txt_extensionEndAngle.Name = "txt_extensionEndAngle";
            this.txt_extensionEndAngle.Size = new System.Drawing.Size(121, 31);
            this.txt_extensionEndAngle.TabIndex = 40;
            // 
            // txt_extensionLength
            // 
            this.txt_extensionLength.AccessibleName = "";
            this.txt_extensionLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_extensionLength.Location = new System.Drawing.Point(572, 395);
            this.txt_extensionLength.Name = "txt_extensionLength";
            this.txt_extensionLength.Size = new System.Drawing.Size(121, 31);
            this.txt_extensionLength.TabIndex = 41;
            // 
            // chk_handrail
            // 
            this.chk_handrail.AutoSize = true;
            this.chk_handrail.Checked = true;
            this.chk_handrail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_handrail.Location = new System.Drawing.Point(947, 58);
            this.chk_handrail.Name = "chk_handrail";
            this.chk_handrail.Size = new System.Drawing.Size(65, 17);
            this.chk_handrail.TabIndex = 42;
            this.chk_handrail.Text = "Handrail";
            this.chk_handrail.UseVisualStyleBackColor = true;
            // 
            // chk_platform
            // 
            this.chk_platform.AutoSize = true;
            this.chk_platform.Checked = true;
            this.chk_platform.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_platform.Location = new System.Drawing.Point(947, 37);
            this.chk_platform.Name = "chk_platform";
            this.chk_platform.Size = new System.Drawing.Size(64, 17);
            this.chk_platform.TabIndex = 43;
            this.chk_platform.Text = "Platform";
            this.chk_platform.UseVisualStyleBackColor = true;
            // 
            // chk_floorSteel
            // 
            this.chk_floorSteel.AutoSize = true;
            this.chk_floorSteel.Checked = true;
            this.chk_floorSteel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_floorSteel.Location = new System.Drawing.Point(947, 81);
            this.chk_floorSteel.Name = "chk_floorSteel";
            this.chk_floorSteel.Size = new System.Drawing.Size(76, 17);
            this.chk_floorSteel.TabIndex = 44;
            this.chk_floorSteel.Text = "Floor Steel";
            this.chk_floorSteel.UseVisualStyleBackColor = true;
            this.chk_floorSteel.CheckedChanged += new System.EventHandler(this.chk_floorSteel_CheckedChanged);
            // 
            // inputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1182, 661);
            this.Controls.Add(this.chk_floorSteel);
            this.Controls.Add(this.chk_platform);
            this.Controls.Add(this.chk_handrail);
            this.Controls.Add(this.txt_extensionLength);
            this.Controls.Add(this.txt_extensionEndAngle);
            this.Controls.Add(this.txt_extensionStartAngle);
            this.Controls.Add(this.txt_platformLength);
            this.Controls.Add(this.txt_platformEndAngle);
            this.Controls.Add(this.txt_platformStartAngle);
            this.Controls.Add(this.lbl_platformExtension);
            this.Controls.Add(this.lbl_platform);
            this.Controls.Add(this.lbl_length);
            this.Controls.Add(this.lbl_endAngle);
            this.Controls.Add(this.lbl_startAngle);
            this.Controls.Add(this.lbl_seg3);
            this.Controls.Add(this.lbl_seg2);
            this.Controls.Add(this.lbl_seg1);
            this.Controls.Add(this.lbl_stackSegments);
            this.Controls.Add(this.txt_thickness3);
            this.Controls.Add(this.txt_thickness2);
            this.Controls.Add(this.txt_height3);
            this.Controls.Add(this.txt_height2);
            this.Controls.Add(this.txt_bottomDiameter3);
            this.Controls.Add(this.txt_bottomDiameter2);
            this.Controls.Add(this.txt_topDiameter3);
            this.Controls.Add(this.txt_topDiameter2);
            this.Controls.Add(this.lbl_height);
            this.Controls.Add(this.lbl_bottomDiameter);
            this.Controls.Add(this.txt_height1);
            this.Controls.Add(this.txt_bottomDiameter1);
            this.Controls.Add(this.txt_thickness1);
            this.Controls.Add(this.lbl_thickness);
            this.Controls.Add(this.lbl_topDiameter);
            this.Controls.Add(this.txt_topDiameter1);
            this.Controls.Add(this.btn_createModel);
            this.Controls.Add(this.txt_originZ);
            this.Controls.Add(this.lbl_originZ);
            this.Controls.Add(this.txt_originY);
            this.Controls.Add(this.lbl_originY);
            this.Controls.Add(this.txt_originX);
            this.Controls.Add(this.lbl_originX);
            this.Name = "inputForm";
            this.Text = "Input Form";
            this.Load += new System.EventHandler(this.inputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_originX;
        private System.Windows.Forms.Label lbl_originY;
        private System.Windows.Forms.Label lbl_originZ;
        public System.Windows.Forms.TextBox txt_originX;
        public System.Windows.Forms.TextBox txt_originY;
        public System.Windows.Forms.TextBox txt_originZ;
        private System.Windows.Forms.Button btn_createModel;
        public System.Windows.Forms.TextBox txt_topDiameter1;
        private System.Windows.Forms.Label lbl_topDiameter;
        private System.Windows.Forms.Label lbl_thickness;
        public System.Windows.Forms.TextBox txt_thickness1;
        public System.Windows.Forms.TextBox txt_bottomDiameter1;
        public System.Windows.Forms.TextBox txt_height1;
        private System.Windows.Forms.Label lbl_bottomDiameter;
        private System.Windows.Forms.Label lbl_height;
        public System.Windows.Forms.TextBox txt_topDiameter2;
        public System.Windows.Forms.TextBox txt_topDiameter3;
        public System.Windows.Forms.TextBox txt_bottomDiameter2;
        public System.Windows.Forms.TextBox txt_bottomDiameter3;
        public System.Windows.Forms.TextBox txt_height2;
        public System.Windows.Forms.TextBox txt_height3;
        public System.Windows.Forms.TextBox txt_thickness2;
        public System.Windows.Forms.TextBox txt_thickness3;
        private System.Windows.Forms.Label lbl_stackSegments;
        private System.Windows.Forms.Label lbl_seg1;
        private System.Windows.Forms.Label lbl_seg2;
        private System.Windows.Forms.Label lbl_seg3;
        private System.Windows.Forms.Label lbl_startAngle;
        private System.Windows.Forms.Label lbl_endAngle;
        private System.Windows.Forms.Label lbl_length;
        private System.Windows.Forms.Label lbl_platform;
        private System.Windows.Forms.Label lbl_platformExtension;
        public System.Windows.Forms.TextBox txt_platformStartAngle;
        public System.Windows.Forms.TextBox txt_platformEndAngle;
        public System.Windows.Forms.TextBox txt_platformLength;
        public System.Windows.Forms.TextBox txt_extensionStartAngle;
        public System.Windows.Forms.TextBox txt_extensionEndAngle;
        public System.Windows.Forms.TextBox txt_extensionLength;
        private System.Windows.Forms.CheckBox chk_handrail;
        private System.Windows.Forms.CheckBox chk_platform;
        private System.Windows.Forms.CheckBox chk_floorSteel;
    }
}

