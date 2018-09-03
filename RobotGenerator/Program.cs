using System;
using NXOpen;
using NXOpen.UF;
using RobotGeneratorWizard;
using System.Windows.Forms;
using System.IO;
using NXOpen.Assemblies;

public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUfSession;
    Form1 form1;
    private static string RobotModelPath;
    public static Program theProgram;
    public static bool isDisposeCalled;


    //------------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------------
    public Program()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();
            isDisposeCalled = false;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            // UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
        }
    }

    //------------------------------------------------------------------------------
    //  Explicit Activation
    //      This entry point is used to activate the application explicitly
    //------------------------------------------------------------------------------
    public static int Main(string[] args)
    {
        int retValue = 0;
        try
        {
            theProgram = new Program();

            theProgram.form1 = new Form1();
            theProgram.form1.OnRobotModelPathSet += theProgram.form1_OnRobotModelPathSet;
            theProgram.form1.ShowDialog();

            //TODO: Add your application code here 

            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
        return retValue;
    }

    void form1_OnRobotModelPathSet(object sender, ModelRobotPathSetEventArgs e)
    {

        theSession.Parts.CloseAll(NXOpen.BasePart.CloseModified.CloseModified, null);
        Part workPart = null;
        Part displayPart = null;

        RobotModelPath = e.RobotModelPath;
        string RobotModelFolderName = RobotModelPath.Substring(RobotModelPath.IndexOf('_') + 1).Remove(RobotModelPath.LastIndexOf('.') - 1);
        string ModelFullPath = string.Format(@"{0}\{1}\{2}\{3}", Directory.GetCurrentDirectory(), "Models_prt", RobotModelFolderName, RobotModelPath);
        PartLoadStatus partLoadStatus;

        BasePart basePart = theSession.Parts.OpenBaseDisplay(ModelFullPath, out partLoadStatus);
        workPart = theSession.Parts.Work;
        displayPart = theSession.Parts.Display;
        partLoadStatus.Dispose();
        
        theProgram.form1.Hide();
    }

    //------------------------------------------------------------------------------
    // Following method disposes all the class members
    //------------------------------------------------------------------------------
    public void Dispose()
    {
        try
        {
            if (isDisposeCalled == false)
            {
                //TODO: Add your application code here 
            }
            isDisposeCalled = true;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
    }

    public static int GetUnloadOption(string arg)
    {
        //Unloads the image explicitly, via an unload dialog
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

        //Unloads the image immediately after execution within NX
        return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

        //Unloads the image when the NX session terminates
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }

}

