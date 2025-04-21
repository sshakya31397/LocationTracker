﻿using SQLite;
using System;

namespace LocationHeatMap.Models
{
    public class LocationEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}