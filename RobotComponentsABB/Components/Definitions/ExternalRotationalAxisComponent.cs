﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using RobotComponentsABB.Parameters.Definitions;
using RobotComponents.BaseClasses.Definitions;

namespace RobotComponentsABB.Components.Definitions
{
    /// <summary>
    /// RobotComponents External Rotational Axis component. An inherent from the GH_Component Class.
    /// </summary>
    public class ExternalRotationalAxisComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public constructor without any arguments.
        /// Category represents the Tab in which the component will appear, Subcategory the panel. 
        /// If you use non-existing tab or panel names, new tabs/panels will automatically be created.
        /// </summary>
        public ExternalRotationalAxisComponent()
          : base("External Rotational Axis", "External Rotational Axis",
              "Defines an External Rotational Axis."
                + System.Environment.NewLine +
                "RobotComponents : v" + RobotComponents.Utils.VersionNumbering.CurrentVersion,
              "RobotComponents", "Definitions")
        {
        }

        /// <summary>
        /// Override the component exposure (makes the tab subcategory).
        /// Can be set to hidden, primary, secondary, tertiary, quarternary, quinary, senary, septenary, dropdown and obscure
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Axis name as a Text", GH_ParamAccess.item, "default_era");
            pManager.AddPlaneParameter("Axis Plane", "AP", "Axis Plane as a Plane", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Axis Limits", "AL", "Axis Limits as Domain", GH_ParamAccess.item);
            pManager.AddMeshParameter("Base Mesh", "BM", "Base Mesh as Mesh", GH_ParamAccess.list);
            pManager.AddMeshParameter("Link Mesh", "LM", "Link Mesh as Mesh", GH_ParamAccess.list);

            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new ExternalRotationalAxisParameter(), "External Rotational Axis", "ERA", "Resulting External Rotational Axis");  //Todo: beef this up to be more informative.
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Input variables
            string name = "";
            Plane axisPlane= Plane.WorldXY;
            Interval limits = new Interval(0, 0);
            List<Mesh> baseMeshes = new List<Mesh>();
            List<Mesh> linkMeshes = new List<Mesh>();
            
            // Catch the input data
            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref axisPlane)) { return; }
            if (!DA.GetData(2, ref limits)) { return; }
            if (!DA.GetDataList(3, baseMeshes)) {  }
            if (!DA.GetDataList(4, linkMeshes)) {  }

            // Make variables needed to join the base and link to one mesh
            Mesh baseMesh = new Mesh();
            Mesh linkMesh = new Mesh();

            // Join the base meshes to one mesh
            for (int i = 0; i < baseMeshes.Count; i++)
            {
                baseMesh.Append(baseMeshes[i]);
            }

            // Join the link meshes to one mesh
            for (int i = 0; i < linkMeshes.Count; i++)
            {
                linkMesh.Append(linkMeshes[i]);
            }

            // Create the external linear axis
            ExternalRotationalAxis externalRotationalAxis = new ExternalRotationalAxis(name, axisPlane, limits, baseMesh, linkMesh);

            // Output
            DA.SetData(0, externalRotationalAxis);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return null; }
            // get { return Properties.Resources.ExternalRotationalAxis_Icon; } //TODO
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("21E3D4EE-18F7-4DCB-AF08-C537A656078D"); }
        }
    }
}