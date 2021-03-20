﻿using MonoGame.SplineFlower.Spline.Types;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MonoGame.SplineFlower.Samples
{
    public partial class SplineForm : Form
    {
        private void toolStripDropDownButtonTwitter_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/SandboxBlizz");
        }

        private void toolStripDropDownButtonGitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sqrMin1/MonoGame.SplineFlower");
        }

        public SplineForm()
        {
            InitializeComponent();
        }
        private void MySpline_TangentSelected(int index)
        {
            labelSelectedTangent.Text = $"Tangent #{index}";
            labelSelectedTangent.Visible = true;
        }
        private void MySpline_TangentDeselected()
        {
            labelSelectedTangent.Text = "";
            labelSelectedTangent.Visible = false;
        }

        private void SplineEditorForm_Load(object sender, EventArgs e)
        {
            comboBoxWalkerMode.SelectedIndex = 0;
            comboBoxCenterTransformMode.SelectedIndex = 3;
            comboBoxCenterTransformMode_2.SelectedIndex = 3;

            hermiteSplineControl.MySpline.TangentSelected += MySpline_TangentSelected;
            hermiteSplineControl.MySpline.TangentDeselected += MySpline_TangentDeselected;
        }

        private void buttonAddCurve_Click(object sender, EventArgs e)
        {
            if (splineControl != null && splineControl.MySpline != null) splineControl.MySpline.AddCurveLeft();
        }

        private void buttonAddCurveRight_Click(object sender, EventArgs e)
        {
            if (splineControl != null && splineControl.MySpline != null) splineControl.MySpline.AddCurveRight();
        }

        private void buttonLoop_Click(object sender, EventArgs e)
        {
            if (splineControl != null && splineControl.MySpline != null)
            {
                splineControl.MySpline.Loop = true;
                buttonLoop.Enabled = false;
            }
        }

        private void buttonResetSplineWalker_Click(object sender, EventArgs e)
        {
            if (splineControl != null &&
                splineControl.MySplineWalker != null &&
                splineControl.MySplineWalker.Initialized)
            {
                splineControl.MySplineWalker.Reset();
            }
        }

        private void comboBoxWalkerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (splineControl != null &&
                splineControl.MySplineWalker != null &&
                splineControl.MySplineWalker.Initialized)
            {
                splineControl.MySplineWalker.WalkerMode = (SplineWalker.SplineWalkerMode)comboBoxWalkerMode.SelectedIndex;
            }
        }

        private void comboBoxCenterTransformMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (splineControl != null &&
                splineControl.MySplineWalker != null &&
                splineControl.MySplineWalker.Initialized)
            {
                splineControl.SetCenterTransformMode = (Controls.TransformControl.CenterTransformMode)comboBoxCenterTransformMode.SelectedIndex;
            }
        }

        private void comboBoxCenterTransformMode_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (catMulRomSpline != null &&
                catMulRomSpline.MySplineWalker != null &&
                catMulRomSpline.MySplineWalker.Initialized)
            {
                catMulRomSpline.SetCenterTransformMode = (Controls.TransformControl.CenterTransformMode)comboBoxCenterTransformMode_2.SelectedIndex;
            }
        }

        private void buttonLoopFindTest_Click(object sender, EventArgs e)
        {
            if (findNearestPointControl1 != null && findNearestPointControl1.MySpline != null)
            {
                findNearestPointControl1.MySpline.Loop = true;
                buttonLoopFindTest.Enabled = false;
            }
        }

        private void buttonCatMulRomFindTest_Click(object sender, EventArgs e)
        {
            if (findNearestPointControl1 != null && findNearestPointControl1.MySpline != null)
            {
                if (!buttonLoopFindTest.Enabled && findNearestPointControl1.MySpline.Loop)
                {
                    CreateFindNearestPointSplines();
                    buttonLoopFindTest.Enabled = true;
                }
                else CreateFindNearestPointSplines();
            }
        }
        private void CreateFindNearestPointSplines()
        {
            if (findNearestPointControl1.MySpline is BezierSpline)
            {
                findNearestPointControl1.CreateCatMulRomSpline();
                buttonCatMulRomFindTest.Text = "Bezier";
            }
            else
            {
                findNearestPointControl1.CreateBezierSpline();
                buttonCatMulRomFindTest.Text = "CatMulRom";
            }
        }

        private void numericUpDownAccuracy_ValueChanged(object sender, EventArgs e)
        {
            if (findNearestPointControl1 != null && findNearestPointControl1.MySpline != null)
            {
                findNearestPointControl1.NearestPointAccuracy = (float)numericUpDownAccuracy.Value;
            }
        }

        private void SplineEditorForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized) UpdateControls();
        }
        private void UpdateControls()
        {
            if (catMulRomSpline != null)
            {
                catMulRomSpline.SplineControl_RecalculateSplineCenter();
                catMulRomSpline.CenterSpline();
            }

            if (splineControl != null)
            {
                splineControl.SplineControl_RecalculateSplineCenter();
                splineControl.CenterSpline();
            }
        }

        private void SplineEditorForm_ResizeEnd(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please edit the Initialization method of the AdvancedControls.cs file to experience all the new ipnut features!\n\nKeyboard Forward: W, D, Up, Right\nKeyboard Backward: S, A, Down, Left\n\nGamePad Forward: DPadUp, DPadRight, RightTrigger, LeftThumbstickUp LeftThumbstickRight\nGamePad Backward: DPadDown, DPadLeft, LeftTrigger, LeftThumbstickDown, LeftThumbstickLeft\n\nTop down vehicle © Copyright by irmirx @ opengameart.org CC-BY 3.0", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonAddTension_Click(object sender, EventArgs e)
        {
            hermiteSplineControl.MySpline.AddTension();
        }

        private void buttonSubstractTension_Click(object sender, EventArgs e)
        {
            hermiteSplineControl.MySpline.SubstractTension();
        }

        private void buttonAddBias_Click(object sender, EventArgs e)
        {
            hermiteSplineControl.MySpline.AddBias();
        }

        private void buttonSubstractBias_Click(object sender, EventArgs e)
        {
            hermiteSplineControl.MySpline.SubstractBias();
        }
    }
}
