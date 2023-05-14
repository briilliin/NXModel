using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NXOpen;
using NXOpenUI;
using NXOpen.UF;
using NXOpen.Utilities;


public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUFSession;
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
            theUFSession = UFSession.GetUFSession();
                        isDisposeCalled = false;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
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

            Tag UFPart;

            string part_name = "Sample1";
            int units1 = 1;
            theUFSession.Part.New(part_name, units1, out UFPart);

            UFCurve.Line Line1 = new UFCurve.Line();
            UFCurve.Line Line2 = new UFCurve.Line();
            UFCurve.Line Line3 = new UFCurve.Line();
            UFCurve.Line Line4 = new UFCurve.Line();

            Line1.start_point = new double[3] { 0, 0, 0 };
            Line1.end_point = new double[3] { 0, 20, 0 };
            Line2.start_point = Line1.end_point;
            Line2.end_point = new double[3] { 20, 20, 0 };
            Line3.start_point = Line2.end_point;
            Line3.end_point = new double[3] { 20, 0, 0 };
            Line4.start_point = Line3.end_point;
            Line4.end_point = new double[3] { 0, 0, 0 };


            Tag[] SK1 = new Tag[4];

            theUFSession.Curve.CreateLine(ref Line1, out SK1[0]);
            theUFSession.Curve.CreateLine(ref Line2, out SK1[1]);
            theUFSession.Curve.CreateLine(ref Line3, out SK1[2]);
            theUFSession.Curve.CreateLine(ref Line4, out SK1[3]);
            NXOpen.Guide.InfoWrite("Построили элементы эскиза");
            string[] Ext1_lim = { "0", "20" };
            double[] point1 = new double[3];
            double[] Ext1_dir = { 0, 0, 1 };


            Tag[] Ext1;
            theUFSession.Modl.CreateExtruded(SK1, "0", Ext1_lim, point1, Ext1_dir, FeatureSigns.Nullsign, out Ext1);
            NXOpen.Guide.InfoWrite("\n Построили элемент вытягивания");

            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
        return retValue;
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
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

        //Unloads the image when the NX session terminates
        return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }

}

