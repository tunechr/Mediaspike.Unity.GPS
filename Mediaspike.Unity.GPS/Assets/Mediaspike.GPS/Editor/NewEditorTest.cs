//using UnityEngine;
//using UnityEditor;
//using NUnit.Framework;
//using EasyRoads3Dv3;
//using System.Collections.Generic;
//using Gpx;
//using System.IO;
//using System.Text;

//public class NewEditorTest {

//    public static ERRoadNetwork roadNetwork;
//    public static ERRoad road;

//    public static GameObject go;
//    public int currentElement = 0;
//    public float distance = 0;
//    public float speed = 5f;

//    //public static IEnumerable<IEnumerable<T>> ToInMemoryBatches<T>(this IEnumerable<T> source, int batchSize)
//    //{
//    //    List<T> batch = null;
//    //    foreach (var item in source)
//    //    {
//    //        if (batch == null)
//    //            batch = new List<T>();

//    //        batch.Add(item);

//    //        if (batch.Count != batchSize)
//    //            continue;

//    //        yield return batch;
//    //        batch = null;
//    //    }

//    //    if (batch != null)
//    //        yield return batch;
//    //}

//    [MenuItem("MyTools/Create East Roads")]
//    public static void CreateEasyRoad()
//    {
//        roadNetwork = new ERRoadNetwork();

        
        
//        ERRoadType roadType = new ERRoadType();
//       // roadType.
//        roadType.roadWidth = 6;
//        roadType.roadMaterial = Resources.Load("Materials/roads/single lane") as Material;
//        // optional
//        roadType.layer = 1;
//        roadType.tag = "Untagged";

//        IList<Coordinates> lst = GetGPXPoints();
//        Debug.Log("List of GPX points converted to Coord:" + lst.Count);

//        Coordinates origin = lst[0];
//        Coordinates.setWorldOrigin(origin);

//        List<Vector3> lstV3 = new List<Vector3>();
//        int n = 0;
//        int roadNum = 0;


//        foreach (Coordinates item in lst)
//        {
//            if (n == 100)
//            {
//                //create a new road and reset
//                Vector3[] markers = lstV3.ToArray();
//                road = roadNetwork.CreateRoad("road "+ roadNum++, roadType, markers);

//                //reset
//                lstV3 = new List<Vector3>();
//                n = 0;
//            }

//            Vector3 current = item.convertCoordinateToVector();
//            Vector3 v = current;
//            Coordinates currentLocation2 = Coordinates.convertVectorToCoordinates(v);
//            currentLocation2.altitude = 0;
//            // v = current + new Vector3(0, 0, 0.4f);
//             v = current + new Vector3(0, (float)item.altitude, 0.4f);
//            //v = current + new Vector3(0, 0, 0f);
//            currentLocation2 = Coordinates.convertVectorToCoordinates(v);

//            lstV3.Add(v);
//            Debug.Log(v.x+ " "+v.x +" " +v.z);
//            n++;
//            if (n > 100) break;
            
            
//        }
        
         
//        road.SnapToTerrain ( false);
//        road.SetResolution(100);
        
//        // road.SetResolution(float value):void;

        
//        // Add Marker: ERRoad.AddMarker(Vector3);
//       // road.AddMarker(new Vector3(300, 5, 300));

//        // Add Marker: ERRoad.InsertMarker(Vector3);
//       // road.InsertMarker(new Vector3(275, 5, 235));
//        //  road.InsertMarkerAt(Vector3 pos, int index): void;

//        // Delete Marker: ERRoad.DeleteMarker(int index);
//       // road.DeleteMarker(2);


//        // Set the road width : ERRoad.SetWidth(float width);
//        //	road.SetWidth(10);

//        // Set the road material : ERRoad.SetMaterial(Material path);
//        //	Material mat = Resources.Load("Materials/roads/single lane") as Material;
//        //	road.SetMaterial(mat);

//        // add / remove a meshCollider component
//        //   road.SetMeshCollider(bool value):void;

//        // set the position of a marker
//        //   road.SetMarkerPosition(int index, Vector3 position):void;
//        //   road.SetMarkerPositions(Vector3[] position):void;
//        //   road.SetMarkerPositions(Vector3[] position, int index):void;

//        // get the position of a marker
//        //   road.GetMarkerPosition(int index):Vector3;

//        // get the position of a marker
//        //   road.GetMarkerPositions():Vector3[];

//        // Set the layer
//        //   road.SetLayer(int value):void;

//        // Set the tag
//        //   road.SetTag(string value):void;

//        // set marker control type
//        //  road.SetMarkerControlType(int marker, ERMarkerControlType type) : bool; // Spline, StraightXZ, StraightXZY, Circular

//        // find a road
//        //  public static function ERRoadNetwork.GetRoadByName(string name) : ERRoad;

//        // get all roads
//        //  public static function ERRoadNetwork.GetRoads() : ERRoad[];  

//        // snap vertices to the terrain (no terrain deformation)
//        //		road.SnapToTerrain(true);

//        // Build Road Network 
//        roadNetwork.BuildRoadNetwork();

//        // Restore Road Network 
//        //	roadNetwork.RestoreRoadNetwork();

//        // Show / Hide the white surfaces surrounding roads
//        //     public function roadNetwork.HideWhiteSurfaces(bool value) : void;

//        //   road.GetConnectionAtStart(): GameObject;
//        //   road.GetConnectionAtStart(out int connection): GameObject; // connections: 0 = bottom, 1= tip, 2 = left, 3 = right (the same for T crossings)

//        //   road.GetConnectionAtEnd(): GameObject;
//        //   road.GetConnectionAtEnd(out int connection): GameObject; // connections: 0 = bottom, 1= tip, 2 = left, 3 = right (the same for T crossings)

//        // Snap the road vertices to the terrain following the terrain shape (no terrain deformation)
//        //   road.SnapToTerrain(bool value): void;
//        //   road.SnapToTerrain(bool value, float yOffset): void;

//        // get the road length
//        //   road.GetLength() : float;

//        // create dummy object
//       // go = GameObject.CreatePrimitive(PrimitiveType.Cube);
//    }


//    public static IList<Coordinates> GetGPXPoints()
//    {
//        IList<Coordinates> lst = new List<Coordinates>();

//        //Reading the GOX
//        Debug.Log(("Reading GPX"));

         
//        StreamReader input = new StreamReader(@"C:\Users\Chris\Documents\UnityProjects\CreateObject\Assets\M to Warrnambool.gpx", Encoding.Default);
//        // StreamReader input = new StreamReader(@"C:\Users\Chris\Documents\UnityProjects\CreateObject\Assets\gpxfiles\Lansdown.gpx", Encoding.Default);
//        //StreamReader input = new StreamReader(@"C:\Users\Chris\Documents\UnityProjects\CreateObject\Assets\gpxfiles\Heffers.gpx", Encoding.Default);
//        // StreamReader input = new StreamReader(@"C:\Users\Chris\Documents\UnityProjects\CreateObject\Assets\chop.gpx", Encoding.Default);
//        // StreamReader input = new StreamReader(@"C:\Users\Chris\Documents\UnityProjects\CreateObject\Assets\Perth worlds qualifier.gpx", Encoding.Default);

//        MemoryStream m = new MemoryStream();

//        using (GpxReader reader = new GpxReader(input.BaseStream))
//        {
//            using (GpxWriter writer = new GpxWriter(m))
//            {
//                while (reader.Read())
//                {
//                    switch (reader.ObjectType)
//                    {
//                        case GpxObjectType.Metadata:
//                            Debug.Log(("Metadata"));
//                            writer.WriteMetadata(reader.Metadata);
//                            break;
//                        case GpxObjectType.WayPoint:
//                            Debug.Log(("Point"));
//                            writer.WriteWayPoint(reader.WayPoint);
//                            break;
//                        case GpxObjectType.Route:
//                            Debug.Log(("Route"));
//                            writer.WriteRoute(reader.Route);
//                            break;
//                        case GpxObjectType.Track:
//                            Debug.Log(("Track"));
//                            writer.WriteTrack(reader.Track);


//                            //Set a local origin
//                            //GPSEncoder.SetLocalOrigin(new Vector2((float)originCoords.latitude, (float)originCoords.longitude));

//                            //Coordinates origin = new  Coordinates(1.289639, -3532648, 1);


//                            IList<GpxTrackSegment> segments = reader.Track.Segments;
//                            IList<GpxTrackPoint> tp = segments[0].TrackPoints;
//                            foreach (GpxTrackPoint item in tp)
//                            {
//                                Debug.Log(("lat" + item.Latitude));
//                                Debug.Log(("lng" + item.Longitude));
//                                Debug.Log(("e" + item.Elevation));

//                                //Coordinates c = new Coordinates(item.Latitude, item.Longitude, item.Elevation.Value);
//                                //Vector3 v = c.convertCoordinateToVector();
//                                Coordinates currentLocation;
//                                currentLocation = new Coordinates(item.Latitude, item.Longitude, item.Elevation.Value);



//                                lst.Add(currentLocation);

//                                //   UnityEngine.GameObject o = (GameObject)Instantiate(prefab, new Vector3(v.x, v.y, v.z), Quaternion.identity);
//                                //UnityEngine.GameObject o = (GameObject)Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
//                                // o.transform.parent = sourceGO.transform;



//                            }


//                            break;
//                    }
//                }
//            }
//        }


//        return lst;

//    }

//    [Test]
//	public void EditorTest()
//	{
//		//Arrange
//		var gameObject = new GameObject();

//		//Act
//		//Try to rename the GameObject
//		var newGameObjectName = "My game object";
//		gameObject.name = newGameObjectName;

//		//Assert
//		//The object has a new name
//		Assert.AreEqual(newGameObjectName, gameObject.name);
//	}
//}
