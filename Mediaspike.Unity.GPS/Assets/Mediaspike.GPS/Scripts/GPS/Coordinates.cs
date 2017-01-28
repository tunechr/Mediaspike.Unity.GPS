using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

[System.Serializable]
public class Coordinates
{

    public double latitude;
    public double longitude;
    public double altitude;

    //CONSTRUCTORS

    public Coordinates(double latitude, double longitude, double altitude)
    {

        this.latitude = latitude;
        this.longitude = longitude;
        this.altitude = altitude;
    }

    public Coordinates(LocationInfo location)
    {

        this.latitude = location.latitude;
        this.longitude = location.longitude;
        this.altitude = location.altitude;
    }

    public Coordinates(Vector2 tileCoords, int zoom)
    {

        Vector2 tileCenter = TileToWorldPos(tileCoords.x + 0.5, tileCoords.y + 0.5, zoom);
        this.latitude = tileCenter.y;
        this.longitude = tileCenter.x;
        this.altitude = 0;
    }

    //CONVERSIONS

    public void updateLocation(LocationInfo location)
    {

        this.latitude = location.latitude;
        this.longitude = location.longitude;
        this.altitude = location.altitude;
    }

    public float gpsAngle(float longitude_o, float latitude_o)
    { //Given an origin

        if (longitude == longitude_o)
            return 0;
        if (longitude - longitude_o < 0)
        {
            return Mathf.Atan((float)(latitude - latitude_o) / (float)(longitude - longitude_o)) + Mathf.PI;
        }
        else
        {
            return Mathf.Atan((float)(latitude - latitude_o) / (float)(longitude - longitude_o));
        }
    }

    public float DistanceFromPoint(Coordinates pt)
    {
        Vector3 ptV = pt.convertCoordinateToVector();
        Vector3 thisV = convertCoordinateToVector();
        return Vector3.Distance(ptV, thisV);
    }

    public Vector3 convertCoordinateToVector()
    {
        Vector3 converted = GPSEncoder.GPSToUCS(new Vector2((float)latitude, (float)longitude));
        return converted;
    }

    public Vector3 convertCoordinateToVector(float y)
    {
        Vector3 converted = GPSEncoder.GPSToUCS(new Vector2((float)latitude, (float)longitude));
        converted.y = y;
        return converted;
    }

    public Vector2 convertCoordinateToVector2D()
    {
        Vector3 converted = GPSEncoder.GPSToUCS(new Vector2((float)latitude, (float)longitude));
        return new Vector2(converted.x, converted.z);
    }

    ////TILES

    public Vector2 tileCoordinates(int zoom)
    {

        Vector2 tileCoords = WorldToTilePos(longitude, latitude, zoom);
        return tileCoords;
    }

    public Coordinates tileCenter(int zoom)
    {

        Vector2 tileCoords = tileCoordinates(zoom);
        Vector2 tileCenter = TileToWorldPos(tileCoords.x + 0.5, tileCoords.y + 0.5, zoom);
        return new Coordinates(tileCenter.y, tileCenter.x, 0);
    }

    public Coordinates tileOrigin(int zoom)
    {

        Vector2 tileCoords = tileCoordinates(zoom);
        Vector2 tileCenter = TileToWorldPos(tileCoords.x, tileCoords.y, zoom);
        return new Coordinates(tileCenter.y, tileCenter.x, 0);
    }

    public float diagonalLenght(int zoom)
    {

        Coordinates center = tileCenter(zoom);
        Coordinates origin = tileOrigin(zoom);
        return 2 * Mathf.Abs(Vector2.Distance(center.convertCoordinateToVector2D(), origin.convertCoordinateToVector2D()));

    }

    public List<Vector2> adiacentNTiles(int zoom, int buffer)
    {

        List<Vector2> adiacentTiles = new List<Vector2>();
        Vector2 centerTileCoords = tileCoordinates(zoom);

        for (int y = -buffer; y <= buffer; y++)
        {
            for (int x = -buffer; x <= buffer; x++)
            {

                adiacentTiles.Add(new Vector2(centerTileCoords.x + x, centerTileCoords.y + y));
            }
        }
        adiacentTiles.Sort(delegate (Vector2 a, Vector2 b) {

            float distA = Mathf.Abs(Vector2.Distance(centerTileCoords, a));
            float distB = Mathf.Abs(Vector2.Distance(centerTileCoords, b));
            return distA.CompareTo(distB);
        });

        return adiacentTiles;

    }

    ////UTILS

    Vector2 WorldToTilePos(double lon, double lat, int zoom)
    {
        Vector2 p = new Vector2();
        p.x = (int)((lon + 180.0) / 360.0 * (1 << zoom));
        p.y = (int)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
            1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

        return p;
    }

    Vector2 TileToWorldPos(double tile_x, double tile_y, int zoom)
    {
        Vector2 p = new Vector2();
        double n = Math.PI - ((2.0 * Math.PI * tile_y) / Math.Pow(2.0, zoom));

        p.x = (float)((tile_x / Math.Pow(2.0, zoom) * 360.0) - 180.0);
        p.y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

        return p;
    }

    ////STATIC

    public static Coordinates convertVectorToCoordinates(Vector3 vector)
    {

        Coordinates coordinates = new Coordinates(0, 0, 0);

        Vector2 latlon = GPSEncoder.USCToGPS(vector);
        coordinates.latitude = latlon.x;
        coordinates.longitude = latlon.y;

        return coordinates;
    }

    public static void setWorldOrigin(Coordinates originCoords)
    {

        GPSEncoder.SetLocalOrigin(new Vector2((float)originCoords.latitude, (float)originCoords.longitude));
    }

}

