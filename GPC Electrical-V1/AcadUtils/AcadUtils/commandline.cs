// CommandLine.cs  Copyright (c) 2006  www.caddzone.com
//
// Original source location:
//
// 	http://www.caddzone.com/CommandLine.cs
//
// LICENSE:
//
// This software may not be published or reproduced in any
// form, without the express written consent of CADDZONE.COM
//
// Fair use:
//
// Permission to use this software in object code form, for
// private software development purposes, and without fee is
// hereby granted, provided that the above copyright notice
// appears in all copies and that both that copyright notice
// and limited warranty and restricted rights notice below
// appear in all supporting documentation.
//
// CADDZONE.COM PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// CADDZONE.COM SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  CADDZONE.COM
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using System.Security;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System.Collections;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Collections.Generic;

namespace AcadUtils
{
    [SuppressUnmanagedCodeSecurity]
    public static class CommandLine
    {
        const string ACAD_EXE = "acad.exe";

        const short RTSTR = 5005;
        const short RTNORM = 5100;
        const short RTNONE = 5000;
        const short RTREAL = 5001;
        const short RT3DPOINT = 5009;
        const short RTLONG = 5010;
        const short RTSHORT = 5003;
        const short RTENAME = 5006;
        const short RTPOINT = 5002;      /*2D point X and Y only */

        static Dictionary<Type, short> resTypes = new Dictionary<Type, short>();

        static CommandLine()
        {
            resTypes[typeof(string)] = RTSTR;
            resTypes[typeof(double)] = RTREAL;
            resTypes[typeof(Point3d)] = RT3DPOINT;
            resTypes[typeof(ObjectId)] = RTENAME;
            resTypes[typeof(Int32)] = RTLONG;
            resTypes[typeof(Int16)] = RTSHORT;
            resTypes[typeof(Point2d)] = RTPOINT;
        }

        static TypedValue TypedValueFromObject(Object val)
        {
            if (val == null)
                throw new ArgumentException("null not permitted as command argument");
            short code = -1;
            if (resTypes.TryGetValue(val.GetType(), out code) && code > 0)
                return new TypedValue(code, val);
            throw new InvalidOperationException("Unsupported type in Command() method");
        }

        public static int Command(params object[] args)
        {
            if (AcadApp.DocumentManager.IsApplicationContext)
                throw new InvalidCastException("Invalid execution context");
            int stat = 0;
            int cnt = 0;
            using (ResultBuffer buffer = new ResultBuffer())
            {
                foreach (object o in args)
                {
                    buffer.Add(TypedValueFromObject(o));
                    ++cnt;
                }
                if (cnt > 0)
                {
                    stat = acedCmd(buffer.UnmanagedObject);
                }
            }
            return stat;
        }

        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        extern static int acedCmd(IntPtr resbuf);

    }

    public class Examples
    {
        // Sample member functions that use the Command() method.

        public static int ZoomExtents()
        {
            return CommandLine.Command("._ZOOM", "_E");
        }

        public static int ZoomCenter(Point3d center, double height)
        {
            return CommandLine.Command("._ZOOM", "_C", center, height);
        }

        public static int ZoomWindow(Point3d corner1, Point3d corner2)
        {
            return CommandLine.Command("._ZOOM", "_W", corner1, corner2);
        }
    }

}


