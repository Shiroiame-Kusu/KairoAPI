﻿namespace KairoAPI.Components
{
    public class VersionUpdateLog
    {
        public string? Version;
        public string? VersionCode;
        public string? Channel;
        public int? Subversion;
        public List<string> UpdatedWhat = [];
        public int ImportantLevel;
    }
}
