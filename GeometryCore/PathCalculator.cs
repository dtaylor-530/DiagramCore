using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeometryCore
{

    public enum PathLine { Straight = 1, Orthogonal = 2 }

    public static class PathCalculator
    {
        //public static List<Point2D> FindPath(Line2D lineA, Line2D lineB, PathLine pathLine = PathLine.Orthogonal, double angle = 0)
        //{
        //    List<Point2D> linkPoints = new List<Point2D>();
        //    linkPoints.Add(lineA.StartPoint);

        //    switch (pathLine)
        //    {
        //        case PathLine.Straight:
        //            break;
        //        case PathLine.Orthogonal:
        //            DetermineOrthogonalPoints(lineA, lineB, ref linkPoints);
        //            break;
        //    }

        //    linkPoints.Add(lineB.StartPoint);
        //    if (angle != 0)
        //    {
        //        List<Point2D> bpoints = null;
        //        switch (pathLine)
        //        {
        //            case PathLine.Straight:
        //                double radius = lineA.StartPoint.DetermineArcRadius(lineB.StartPoint, angle);
        //                bpoints = GetArcGeometry(lineA.StartPoint, lineB.StartPoint, radius).ToPointsList();
        //                break;
        //            case PathLine.Orthogonal:
        //                bpoints = GetBezierGeometry(linkPoints).ToPointsList();
        //                break;
        //        }
        //        linkPoints.Clear();
        //        foreach (var point in bpoints)
        //            linkPoints.Add(point);
        //    }
        //    return linkPoints;
        //}





        public static void DetermineOrthogonalPoints(Line2D lineA, Line2D lineB, ref List<Point2D> linkPoints)
        {


            Line2D[] primarySet = new Line2D[2];
            Line2D[] secondarySet = new Line2D[2];

            primarySet[0] = lineA;
            primarySet[1] = lineB;


            secondarySet[0] = new Line2D(new Point2D(primarySet[0].StartPoint.X, primarySet[1].StartPoint.Y), new Point2D(primarySet[0].StartPoint.X, primarySet[1].StartPoint.Y));
            secondarySet[1] = new Line2D(new Point2D(primarySet[1].StartPoint.X, primarySet[0].StartPoint.Y), new Point2D(primarySet[1].StartPoint.X, primarySet[0].StartPoint.Y));



            Vector2D CombinedVector = primarySet[0].ToVector().Add(primarySet[1].ToVector());

            //double angle = Vector.AngleBetween(CombinedVector, new Vector(1, 0));

            /*
            'The velocity of object 2 relative to object 1 is given by v:=v2−v1v:=v2−v1.'
            (N.B in our case direction is equivalent to velocity)*/
            Vector2D PrimaryDifferenceVector = primarySet[0].ToVector().Subtract(primarySet[1].ToVector());

            //The displacement of object 2 from object 1 is given by d:=p2−p1d:=p2−p1.
            Vector2D PrimaryDisplacementVector = primarySet[0].StartPoint.ToVector2D().Subtract(primarySet[1].StartPoint.ToVector2D());

            // dot product of the difference and the displacement vectors for the primary vectors ( i.e the ones representing start and end) : 
            //If the result is positive, then the objects are moving away from each other.
            //If the result is negative, then the objects are moving towards each other. 
            //If the result is 0, then the distance is (at that instance) not changing.
            PrimaryDisplacementVector.Normalize(); //neccessary for calculating vectorlength
            double RelativeMovement = PrimaryDisplacementVector.DotProduct(PrimaryDifferenceVector);


            Vector2D vp = PrimaryDifferenceVector.Add(PrimaryDisplacementVector);

            double primaryvectorlength = vp.Length;


            // Primary Loop

            foreach (Line2D primaryVertex in primarySet)
            {

                // direction vector of primary vertex
                Vector2D directionVector = primaryVertex.ToVector();


                foreach (Line2D secondaryVertex in secondarySet)
                {
                    // displacement vector between the primary and secondary vertex
                    Vector2D displacementVector = primaryVertex.StartPoint.ToVector2D().Subtract(secondaryVertex.StartPoint.ToVector2D());
                    displacementVector.Normalize();

                    // type of line depends of relative direction of primary vertices:
                    // if CombinedVector has a length(magnitude), line is basic right angle e.g
                    // |__  __| 
                    // else if CombinedVector is has no length(magnitude), the direction of primary vertices are opposed: line is double right angle e.g
                    // |__      __|
                    //    | or |

                    // check relation of primary vertices to one another with combined vector:
                    // direction is perpindicular
                    if (CombinedVector.X != 0 && CombinedVector.Y != 0)
                    {
                        // for whatever reason the number 2.2  for the Primary Vector Length determines whether the parrallel or perpindular line 
                        // can be used
                        // this is related to the fact that if the direction of the primary vertices are facing inwards they will make any combined vector with 
                        // the displacement vector smaller. Possible related to the fact that 2.2 sqrt is 1.5
                        if (primaryvectorlength > 2.2)
                        {
                            // check if parallel
                            if (!directionVector.IsPerpindicular(displacementVector))
                                if (!linkPoints.Contains(secondaryVertex.StartPoint)) linkPoints.Add(secondaryVertex.StartPoint);
                        }
                        else if (primaryvectorlength < 2.2)
                        {
                            // check if perpindcular
                            if (directionVector.IsPerpindicular(displacementVector))
                                if (!linkPoints.Contains(secondaryVertex.StartPoint)) linkPoints.Add(secondaryVertex.StartPoint);
                        }

                    }
                    // direction is parrallel & same direction
                    else if ((CombinedVector.X == 0 & CombinedVector.Y != 0) || (CombinedVector.Y == 0 & CombinedVector.X != 0))
                    {
                        if (directionVector == displacementVector.GetNormalized())
                            if (!linkPoints.Contains(secondaryVertex.StartPoint)) linkPoints.Add(secondaryVertex.StartPoint);

                    }
                    //  direction is parrallel but different directions
                    else if (CombinedVector.X == 0 & CombinedVector.Y == 0)
                    {
                        // Add parrallel line midpoint

                        // the RelativeMovement determines the type of midpoint selected:
                        // if >0 then midpoint taken from line running parrallel with direction, else <0 then  perpindiclar

                        if (RelativeMovement > 0)
                            // check if parrallel
                            if (!directionVector.IsPerpindicular(displacementVector))
                                linkPoints.Add(primaryVertex.StartPoint.MidPoint(secondaryVertex.StartPoint));
                        // Add perpindicular line midpoint
                        // if line can not be found - with the same direction to that of the primary vertex - 
                        // to connect primary and secondary point  then  midpoint of perpindicular line is added

                        if (RelativeMovement < 0)
                            // check if perpindcular
                            if (directionVector.IsPerpindicular(displacementVector))
                                linkPoints.Add(primaryVertex.StartPoint.MidPoint(secondaryVertex.StartPoint));

                    }
                }
            }
        }
    }
}