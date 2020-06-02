using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DesktopUniversalFrame.Entity
{
    /// <summary>
    /// 算法推导类
    /// </summary>
    public class ArithmeticInfer
    {
        /***
         ***将球面进行三角形拆分,设球面的参数方程为：
         *** x = - r * cosφ * sinθ
         *** y = r * sinφ
         *** z = - r * cos φ * cosθ
         ***其中，-π/2≤φ≤π/2，-2π≤θ≤2π
         ****/
        private const int stacks = 3600; //横向等分数
        private const int Slices = 360; //纵向等分数
        private const double R = 20D;
        private Point3D center = new Point3D(0, 0, 0);
        private MeshGeometry3D GetSphereGeometry3D()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            var normal = new Vector3D();
            double x, y, z, theta, phi;

            for (int i = 0; i < stacks; i++)  //将球面拆分成Stacks个等距离的薄片（纬度，自上向下）
            {
                phi = Math.PI / 2D - (double)i * Math.PI / stacks;  //计算该纬度位置的phi角
                y = R * Math.Sin(phi);
                for (int j = 0; j < Slices; j++)  //将球面纵向切成Slices份（经度方向，自西向东）
                {
                    theta = j * 2D * Math.PI / Slices; //计算该经度位置的theta角                    
                    x = -R * Math.Cos(phi) * Math.Sin(theta);
                    z = -R * Math.Cos(phi) * Math.Cos(theta);
                    normal = new Vector3D(x, y, z);
                    mesh.Positions.Add(normal + center);
                    mesh.TextureCoordinates.Add(new Point((double)j / Slices, (double)i / stacks)); //指定该顶点与二维填充平面的对应关系
                };
            }

            for (int h = 0; h < stacks; h++)
            {
                for (int k = 0; k < Slices + 1; k++)
                {
                    if (k < Slices - 1)
                    {
                        mesh.TriangleIndices.Add(Slices * h + k + 1);
                        mesh.TriangleIndices.Add(Slices * h + k);
                        mesh.TriangleIndices.Add(Slices * (h + 1) + k);

                        mesh.TriangleIndices.Add(Slices * (h + 1) + k);
                        mesh.TriangleIndices.Add(Slices * (h + 1) + k + 1);
                        mesh.TriangleIndices.Add(Slices * h + k + 1);
                    }
                    else
                    {
                        mesh.TriangleIndices.Add(Slices * h + 0);
                        mesh.TriangleIndices.Add(Slices * h + Slices - 1);
                        mesh.TriangleIndices.Add(Slices * (h + 1) + Slices - 1);

                        mesh.TriangleIndices.Add(Slices * (h + 1) + Slices - 1);
                        mesh.TriangleIndices.Add(Slices * (h + 1) + 0);
                        mesh.TriangleIndices.Add(Slices * h + 0);
                    }

                    //mesh.TriangleIndices.Add(Slices * h + k + 1);
                    //mesh.TriangleIndices.Add(Slices * h + k);
                    //mesh.TriangleIndices.Add(Slices * (h + 1) + k);

                    //mesh.TriangleIndices.Add(Slices * (h + 1) + k);
                    //mesh.TriangleIndices.Add(Slices * (h + 1) + k + 1);
                    //mesh.TriangleIndices.Add(Slices * h + k + 1);
                }

                //mesh.TriangleIndices.Add(Slices * h + 0);
                //mesh.TriangleIndices.Add(Slices * h + Slices - 1);
                //mesh.TriangleIndices.Add(Slices * (h + 1) + Slices - 1);

                //mesh.TriangleIndices.Add(Slices * (h + 1) + Slices - 1);
                //mesh.TriangleIndices.Add(Slices * (h + 1) + 0);
                //mesh.TriangleIndices.Add(Slices * h + 0);
            }
            return mesh;
        }

        /***
         *将圆柱体进行三角形剖分
         *空间中圆的参数方程：（x,y,z) = r*(A*cosθ+B*sinθ)+(x0,y0,z0)[0≤θ≤2π]
         *其中 a、b 是单位向量，且满足 A⊥B⊥n（圆的法向量）
         */
        /// <summary>
        /// 两底面圆心在 p1、p2 位置，底面半径为 R 的圆柱体进行三角形剖分
        /// </summary>
        /// <param name="p1">圆柱上底面圆心的坐标</param>
        /// <param name="p2">圆柱下底面圆心的坐标</param>
        /// <param name="R">圆柱的底面半径</param>
        /// <returns></returns>
        private MeshGeometry3D GetCylinderGeometry3D(Point3D p1, Point3D p2, double R)
        {
            MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
            Vector3D CircleVector = p2 - p1; //向量p1p2
            Vector3D M = new Vector3D(1, 1, 1);
            if (Vector3D.AngleBetween(M, CircleVector) < 0.1)
                M = new Vector3D(1, 0, 0);

            Vector3D A = Vector3D.CrossProduct(CircleVector, M);
            Vector3D B = Vector3D.CrossProduct(CircleVector, A);
            A.Normalize();
            B.Normalize();

            double theta;
            Point3D pos1, pos2;

            for (int i = 0; i < stacks; i++)
            {
                theta = i * Math.PI * 2D / stacks;
                pos1 = R * (A * Math.Cos(theta) + B * Math.Sin(theta)) + p2;
                pos2 = R * (A * Math.Cos(theta) + B * Math.Sin(theta)) + p1;

                meshGeometry3D.Positions.Add(pos1);
                meshGeometry3D.Positions.Add(pos2);

                meshGeometry3D.TextureCoordinates.Add(new Point((double)i / stacks, 0));
                meshGeometry3D.TextureCoordinates.Add(new Point((double)i / stacks, 1));
            }

            for (int j = 0; j < stacks - 1; j++)
            {
                meshGeometry3D.TriangleIndices.Add(j * 2);
                meshGeometry3D.TriangleIndices.Add(j * 2 + 1);
                meshGeometry3D.TriangleIndices.Add(j * 2 + 3);

                meshGeometry3D.TriangleIndices.Add(j * 2);
                meshGeometry3D.TriangleIndices.Add(j * 2 + 3);
                meshGeometry3D.TriangleIndices.Add(j * 2 + 2);
            }

            return meshGeometry3D;
        }

        #region 命中测试

        public void HitTest(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            Viewport3D viewport3D = sender as Viewport3D;
            Point mouseposition = args.GetPosition(viewport3D);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
            VisualTreeHelper.HitTest(viewport3D, null, HTResult, pointparams);
        }

        public HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            //MessageBox.Show(rawresult.ToString());

            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            if (rayResult != null)
            {
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
                if (rayMeshResult != null)
                {
                    GeometryModel3D hitgeo = rayMeshResult.ModelHit as GeometryModel3D;

                    MeshGeometry3D hitmesh = hitgeo.Geometry as MeshGeometry3D;
                    Point3D p1 = hitmesh.Positions.ElementAt(rayMeshResult.VertexIndex1);
                    double weight1 = rayMeshResult.VertexWeight1;
                    Point3D p2 = hitmesh.Positions.ElementAt(rayMeshResult.VertexIndex2);
                    double weight2 = rayMeshResult.VertexWeight2;
                    Point3D p3 = hitmesh.Positions.ElementAt(rayMeshResult.VertexIndex3);
                    double weight3 = rayMeshResult.VertexWeight3;
                    Point3D prePoint = new Point3D(p1.X * weight1 + p2.X * weight2 + p3.X * weight3, p1.Y * weight1 + p2.Y * weight2 + p3.Y * weight3, p1.Z * weight1 + p2.Z * weight2 + p3.Z * weight3);

                    //UpdateResultInfo(rayMeshResult);
                    //UpdateMaterial(hitgeo, (side1GeometryModel3D.Material as MaterialGroup));
                }
            }
            return HitTestResultBehavior.Continue;
        }

        #endregion

        ////窗口只适应
        //private static dynamic GetScreenSize(Window window)
        //{
        //    var intPtr = new WindowInteropHelper(window).Handle;//获取当前窗口的句柄
        //    var screen = Screen.FromHandle(intPtr);//获取当前屏幕
        //    using (Graphics currentGraphics = Graphics.FromHwnd(intPtr))
        //    {
        //        //分别获取当前屏幕X/Y方向的DPI
        //        double dpiXRatio = currentGraphics.DpiX / DpiPercent;
        //        double dpiYRatio = currentGraphics.DpiY / DpiPercent;

        //        var width = screen.WorkingArea.Width / dpiXRatio;
        //        var height = screen.WorkingArea.Height / dpiYRatio;

        //        return new
        //        {
        //            Width = width,
        //            Height = height
        //        };
        //    }
        //}
    }
}
