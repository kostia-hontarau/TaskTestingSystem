﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using System.Windows;


namespace TTS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TestingFilesPanel.xaml
    /// </summary>
    public partial class TestingFilesPanel : UserControl
    {
        #region Constructors
        public TestingFilesPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region Propeties
        public string CurrentFile
        {
            get
            {
                TestingFileControl control = this.FilesPanel.Children.OfType<TestingFileControl>()
                    .SingleOrDefault(element => element.Selected);
                if (control != null)
                    return control.FilePath;

                return String.Empty;
            }
        }
        #endregion
        
        #region Events
        public event EventHandler SelectionChanged;
        #endregion

        #region Event Invokators
        protected virtual void OnSelectionChanged()
        {
            EventHandler handler = SelectionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void TestingFileControl_DeleteButtonClick(object sender, System.EventArgs e)
        {
            TestingFileControl control = sender as TestingFileControl;
            if (control != null)
            {
                control.DeleteButtonClick -= TestingFileControl_DeleteButtonClick;
                this.FilesPanel.Children.Remove(control);
            }
        }
        private void testingFileControl_ElementSelected(object sender, System.EventArgs e)
        {
            TestingFileControl selectedControl = sender as TestingFileControl;
            List<TestingFileControl> resetSelectionList =
                this.FilesPanel.Children.OfType<TestingFileControl>()
                .Where(element => element.Selected)
                .ToList();
            foreach (TestingFileControl control in resetSelectionList)
            {
                if (control != selectedControl)
                    control.Selected = false;
            }
            this.OnSelectionChanged();
        }
        #endregion

        #region Members
        public void AddItem(string filePath)
        {
            TestingFileControl testingFileControl = new TestingFileControl(filePath);
            testingFileControl.DeleteButtonClick += TestingFileControl_DeleteButtonClick;
            testingFileControl.ElementSelected += testingFileControl_ElementSelected;
            testingFileControl.Selected = true;

            this.FilesPanel.Children.Add(testingFileControl);
        }
        public List<string> GetSelectedFiles()
        {
            return this.FilesPanel.Children.OfType<TestingFileControl>()
                                           .Where(element => element.FileCheckBox.IsChecked == true)
                                           .Select(element => element.FilePath)
                                           .ToList();
        }
        public List<string> GetFiles()
        {
            return this.FilesPanel.Children.OfType<TestingFileControl>()
                                           .Select(element => element.FilePath)
                                           .ToList();
        }

        public void SelectNextFile()
        {
            List<TestingFileControl> controls = this.FilesPanel.Children.OfType<TestingFileControl>().ToList();
            for (int index = 0; index < controls.Count - 1; index++)
            {
                if (controls[index].Selected)
                {
                    controls[index + 1].Selected = true;
                }
            }
        }
        #endregion
    }
}
