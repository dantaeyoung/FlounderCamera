using System;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Collections;

using GH_IO;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;


namespace getFlounderCamera
{
    public class getFlounderCameraComponent : GH_Component
    {
		bool Activate;
        string ViewportName;
        Point3d Location;
		Point3d Target;
		double Lens;
		Vector3d Up;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public getFlounderCameraComponent()
            : base("getFlounderCamera", "getFlounderCamera",
                "Lets you control a camera by specifiying the viewport name.",
                "Flounder", "Flounder")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Activate", "Activate", "Activate", GH_ParamAccess.item); 
            pManager.AddTextParameter("ViewportName", "ViewportName", "ViewportName", GH_ParamAccess.item); 
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Location", "Location", "Camera Location", GH_ParamAccess.item);
            pManager.AddPointParameter("Target", "Target", "Camera Target", GH_ParamAccess.item);
            pManager.AddNumberParameter("Lens", "Lens", "Camera Lens Focal Length", GH_ParamAccess.item);
            pManager.AddVectorParameter("Up", "Up", "Camera's Up (Z) vector", GH_ParamAccess.item); 
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //  Retrieve input data, exit if non-existent
            if (!DA.GetData(0, ref Activate)) { return; }
            if (!DA.GetData(1, ref ViewportName)) { return; }

            bool toggled = false;

            if (Activate)
            {
                foreach (Rhino.Display.RhinoView thisview in RhinoDoc.ActiveDoc.Views)
                {
                    if (ViewportName == thisview.ActiveViewport.Name)
                    {
                        toggled = true;
                        Location = thisview.ActiveViewport.CameraLocation;
                        Target = thisview.ActiveViewport.CameraTarget;
                        Lens = thisview.ActiveViewport.Camera35mmLensLength;
                        Up = thisview.ActiveViewport.CameraUp;
						DA.SetData(0, Location);
						DA.SetData(1, Target);
						DA.SetData(2, Lens);
						DA.SetData(3, Up);
                    }
                }

                if (toggled == false)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Viewport '" + ViewportName + "' does not exist!");
                }

             }

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resource1.getFlounderCameraIcon;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{344791d3-9765-479a-8bd0-2ee38c9760d8}"); }
        }
    }
}
