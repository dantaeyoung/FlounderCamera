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

namespace setNamedCamera
{
    public class setNamedCameraComponent : GH_Component
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
        public setNamedCameraComponent()
            : base("syncFlounderCamera", "syncFlounderCamera",
                "syncFlounderCamera",
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
            pManager.AddPointParameter("Location", "Location", "Camera Location", GH_ParamAccess.item);
            pManager.AddPointParameter("Target", "Target", "Camera Target", GH_ParamAccess.item);
            pManager.AddNumberParameter("Lens", "Lens", "Camera Lens Focal Length", GH_ParamAccess.item);
            pManager.AddVectorParameter("Up", "Up", "Camera's Up (Z) vector", GH_ParamAccess.item); 
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
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
            if (!DA.GetData(2, ref Location)) { return; }
            if (!DA.GetData(3, ref Target)) { return; }
            if (!DA.GetData(4, ref Lens)) { return; }
            if (!DA.GetData(5, ref Up)) { return; }

            bool toggled = false;

            if (Activate)
            {
                foreach (Rhino.Display.RhinoView thisview in RhinoDoc.ActiveDoc.Views)
                {
                    if (ViewportName == thisview.ActiveViewport.Name)
                    {
                        toggled = true;
                        thisview.ActiveViewport.SetCameraLocations(Target, Location);
                        thisview.ActiveViewport.Camera35mmLensLength = (double) Lens;
                        thisview.ActiveViewport.CameraUp = Up;
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
                return Resource1.setNamedCameraIcon;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{69a5664a-ea89-4492-90c6-c377fb396747}"); }
        }
    }
}
