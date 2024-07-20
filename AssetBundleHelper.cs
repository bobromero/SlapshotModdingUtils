using Il2CppInterop.Runtime;
using UnityEngine;

namespace SlapshotModdingUtils {
    public static class AssetBundleHelper {
        /// <summary>
        /// Loads All assets in YourNameSpace.Assets folder ***README***
        /// use System.Reflection.Assembly.GetExecutingAssembly() as the default for ass and
        /// use this.GetType().Namespace as the default for NameSpace
        /// </summary>
        /// <param name="ass">The current executing assembly, use System.Reflection.Assembly.GetExecutingAssembly() as the default</param>
        /// <param name="NameSpace">The namespace containing your resource, this.GetType().Namespace</param>
        /// <returns>A list prefabs</returns>
        public static List<GameObject> LoadAssets(System.Reflection.Assembly ass, string? NameSpace) {
            List<GameObject> prefabs = new List<GameObject>();

            foreach (var resource in ass.GetManifestResourceNames()) {
                //Melon<TestClass>.Logger.Msg(ass.GetManifestResourceNames()[i++]);
                //Melon<TestClass>.Logger.Msg(this.GetType().Namespace + ".Assets");
                if (!resource.StartsWith(NameSpace + ".Assets")) {
                    continue;
                    //Melon<TestClass>.Logger.Msg("ignoring " + resource + " not in Assets folder");
                }

                var assetBundle = MemoryLoad(ass, resource);
                if (assetBundle == null) {
                    //Melon<TestClass>.Logger.Error("AssetBundle " + resource + " is null");
                } else {
                    assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                    //Melon<TestClass>.Logger.Msg("Loaded Bundle, getting prefab");

                    foreach (var asset in assetBundle.LoadAllAssets()) {
                        try {
                            //Melon<TestClass>.Logger.Msg("getting prefab for asset: " + asset.name);
                            var prefab = assetBundle.LoadAsset(asset.name, Il2CppType.Of<GameObject>()).Cast<GameObject>();
                            prefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                            prefabs.Add(prefab);
                            //Melon<TestClass>.Logger.Msg("Adding prefab: " + prefab.name);
                            //Melon<TestClass>.Logger.Msg("Prefab added");
                        } catch {
                            
                        }

                    }

                }
            }
            return prefabs;
        }

        //public static GameObject? LoadAsset(System.Reflection.Assembly ass, string? NameSpace, string AssetName) {

        //}

        private static AssetBundle? MemoryLoad(System.Reflection.Assembly ass, string? resource) {
            //https://github.com/ddakebono/BTKSASelfPortrait/blob/master/BTKSASelfPortrait.cs
            var memStream = ass.GetManifestResourceStream(resource);
            if (memStream != null) {
                var tempStream = new MemoryStream((int)memStream.Length);
                memStream.CopyTo(tempStream, 65535);
                //Melon<TestClass>.Logger.Msg("Size of " + resource + " is: " + tempStream.Length);

                //Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<byte> pleaseStream;
                var bytes = tempStream.ToArray();
                //Melon<TestClass>.Logger.Msg("Bytes for " + resource + " is: " + bytes);
                return AssetBundle.LoadFromMemory(bytes);

                //byte[] byteArray = new byte[tempStream.Length];

                //AssetBundle assetBundle = AssetBundle.LoadFromMemory(byteArray, (uint)memStream.Read(byteArray, 0, byteArray.Length));

            } else {
                //Melon<TestClass>.Logger.Error("memStream is null");    
            }
            return null;
        }


    }

   
}
