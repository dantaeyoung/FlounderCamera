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

namespace syncFlounderCamera
{
    public class syncFlounderCameraComponent : GH_Component
    {
        bool Activate;
        string SrcViewportName;
        string DestViewportName;
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
        public syncFlounderCameraComponent()
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
            pManager.AddTextParameter("SourceViewportName", "SrcViewportName", "Source Viewport Name", GH_ParamAccess.item); 
            pManager.AddTextParameter("DestinationViewportName", "DestViewportName", "Destination Viewport Name", GH_ParamAccess.item); 

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
            if (!DA.GetData(1, ref SrcViewportName)) { return; }
            if (!DA.GetData(2, ref DestViewportName)) { return; }


            bool srcToggled = false;
            bool destToggled = false;

            Rhino.Display.RhinoViewport srcViewport = null, destViewport = null;

            if (Activate)
            {
                foreach (Rhino.Display.RhinoView thisview in RhinoDoc.ActiveDoc.Views)
                {
                    if (SrcViewportName == thisview.ActiveViewport.Name) {
                        srcViewport = thisview.ActiveViewport;
                        srcToggled = true;
                    }
                    if (DestViewportName == thisview.ActiveViewport.Name) {
                        destViewport = thisview.ActiveViewport;
                        destToggled = true;
                    }
                }

                if (srcToggled == false) {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Viewport '" + SrcViewportName + "' does not exist!");
                }
                if (destToggled == false) {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Viewport '" + DestViewportName + "' does not exist!");
                }
				if(srcToggled == true && destToggled == true) {
					destViewport.SetCameraLocations(srcViewport.CameraTarget, srcViewport.CameraLocation);
					destViewport.Camera35mmLensLength = srcViewport.Camera35mmLensLength;
					destViewport.CameraUp = srcViewport.CameraUp;
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
                return Resource1.syncFlounderCameraIcon;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
			get { return new Guid("{9891d456-4594-4ac1-aba5-ec195c4da6f8}"); }
        }
    }
}
