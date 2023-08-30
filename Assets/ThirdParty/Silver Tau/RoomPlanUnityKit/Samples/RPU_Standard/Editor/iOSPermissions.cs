namespace Marevo 
{
#if UNITY_EDITOR
	
    using UnityEditor;
    using UnityEditor.Callbacks;
    using System.IO;

    #if UNITY_IOS
    using UnityEditor.iOS.Xcode;
    #endif

    public static class iOSPermissions
    {
	    private const string PhotoLibraryUsageKey = @"NSPhotoLibraryAddUsageDescription";
	    private const string PhotoLibraryUsageDescription = @"Allow this app to save videos to your photo library";
	    private const string PhotoLibraryUsage2Key = @"NSPhotoLibraryUsageDescription";
	    private const string PhotoLibraryUsage2Description = @"Allow this app to save videos to your photo library";
	    private const string AppUsesNonExemptEncryptionKey = @"ITSAppUsesNonExemptEncryption";
	    private const bool AppUsesNonExemptEncryptionDescription = false;
        
        #if UNITY_IOS

		[PostProcessBuild]
		static void SetPermissions (BuildTarget buildTarget, string path) {
			if (buildTarget != BuildTarget.iOS) return;
			string plistPath = path + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			PlistElementDict rootDictionary = plist.root;
			rootDictionary.SetString(PhotoLibraryUsageKey, PhotoLibraryUsageDescription);
			rootDictionary.SetBoolean(AppUsesNonExemptEncryptionKey, AppUsesNonExemptEncryptionDescription);
			File.WriteAllText(plistPath, plist.WriteToString());
		}
		#endif
    }

#endif
}
#pragma warning restore 0162, 0429