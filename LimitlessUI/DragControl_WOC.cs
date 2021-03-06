﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

public class DragControl_WOC : Component
{
    [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
    private static extern bool ReleaseCapture();

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HT_CAPTION = 0x2;

    private bool _draggableInnerControls = false;
    private bool _dragType = true;
    private bool _maximiseOnDoubleClick = true;

    private Control _control;


    private void onControlMouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            if (_dragType)
                SendMessage(_control.FindForm().Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            else
                SendMessage(_control.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        if (e.Button == MouseButtons.Left && e.Clicks == 2 && _maximiseOnDoubleClick)
        {
            if (_control.FindForm().WindowState == FormWindowState.Normal)
                _control.FindForm().WindowState = FormWindowState.Maximized;
            else
                _control.FindForm().WindowState = FormWindowState.Normal;
        }
    }

    public Control TargetControl
    {
        get { return _control; }
        set
        {
            _control = value;
            if (value != null)
                _control.MouseDown += onControlMouseDown;
            if (_draggableInnerControls)
                foreach (Control innerControl in _control.Controls)
                    innerControl.MouseDown += onControlMouseDown;
        }
    }


    public bool DraggableInnerControls
    {
        get { return _draggableInnerControls; }
        set { _draggableInnerControls = value; }
    }

    public bool Fixed
    {
        get { return _dragType; }
        set { _dragType = value; }
    }

    public bool MaximiseOnDoubleClick
    {
        get { return _maximiseOnDoubleClick; }
        set { _maximiseOnDoubleClick = value; }
    }
}

