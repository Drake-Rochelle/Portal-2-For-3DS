using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class CiaForgeX : EditorWindow
{
    [MenuItem("3DS/CiaForgeX")]
    public static void ShowWindow()
    {
        var window = GetWindow<CiaForgeX>("CiaForgeX");
        window.position = new Rect(10, 30, 340, 260);
        window.Show();
    }

    public static bool toolEnabled
    {
        get { return EditorPrefs.GetBool("CiaForgeX_toolEnabled", true); }
        private set { EditorPrefs.SetBool("CiaForgeX_toolEnabled", value); }
    }

    public static string toolMode
    {
        get { return EditorPrefs.GetString("CiaForgeX_toolMode", "Extract, Modify and Rebuild CIA"); }
        private set { EditorPrefs.SetString("CiaForgeX_toolMode", value); }
    }

    public static string systemMode
    {
        get { return EditorPrefs.GetString("CiaForgeX_systemMode", "80MB"); }
        private set { EditorPrefs.SetString("CiaForgeX_systemMode", value); }
    }

    public static string n3dsSystemMode
    {
        get { return EditorPrefs.GetString("CiaForgeX_n3dsSystemMode", "Default"); }
        private set { EditorPrefs.SetString("CiaForgeX_n3dsSystemMode", value); }
    }

    public static bool removeCCI
    {
        get { return EditorPrefs.GetBool("CiaForgeX_removeCCI", true); }
        private set { EditorPrefs.SetBool("CiaForgeX_removeCCI", value); }
    }

    public static bool removeXML
    {
        get { return EditorPrefs.GetBool("CiaForgeX_removeXML", true); }
        private set { EditorPrefs.SetBool("CiaForgeX_removeXML", value); }
    }

    void OnGUI()
    {
        GUILayout.Label("General", EditorStyles.boldLabel);
        toolEnabled = EditorGUILayout.Toggle("Enable Tool", toolEnabled);
        if (!toolEnabled) return;

        string[] toolModeOptions = { "Build CIA", "Extract, Modify and Rebuild CIA" };
        int toolModeIndex = System.Array.IndexOf(toolModeOptions, toolMode);
        toolMode = toolModeOptions[EditorGUILayout.Popup("Export Mode ", toolModeIndex, toolModeOptions)];
        if (systemMode == "64MB (Default)" && n3dsSystemMode == "Default" && toolMode == "Extract, Modify and Rebuild CIA") GUILayout.Label("It's recommended to use 'Build CIA' if SystemMode is not changed anyway", EditorStyles.helpBox);

        if (toolMode == "Extract, Modify and Rebuild CIA")
        {
            GUILayout.Space(5);
            GUILayout.Label("Extended Memory", EditorStyles.boldLabel);
            string[] systemModeOptions = { "32MB", "64MB (Default)", "72MB", "80MB", "96MB (NOT WORKING)" };
            int systemModeIndex = System.Array.IndexOf(systemModeOptions, systemMode);
            systemMode = systemModeOptions[EditorGUILayout.Popup("Set SystemMode to", systemModeIndex, systemModeOptions)];
            GUILayout.Space(3);
            GUILayout.Label("New 3DS Extended Memory", EditorStyles.boldLabel);
            string[] n3dsModeOptions = { "Default", "124MB", "178MB" };
            int n3dsModeIndex = System.Array.IndexOf(n3dsModeOptions, n3dsSystemMode);
            n3dsSystemMode = n3dsModeOptions[EditorGUILayout.Popup("Set N3ds SystemMode to", n3dsModeIndex, n3dsModeOptions)];
        }

        GUILayout.Space(5);
        GUILayout.Label("Extra", EditorStyles.boldLabel);
        removeCCI = EditorGUILayout.Toggle("Remove CCI file", removeCCI);
        if (!removeCCI && toolMode == "Extract, Modify and Rebuild CIA" && !(systemMode == "64MB (Default)" && n3dsSystemMode == "Default")) GUILayout.Label("The CCI file itself does not get modified, so it won't have extended memory mode enabled!", EditorStyles.helpBox);
        removeXML = EditorGUILayout.Toggle("Remove XML file", removeXML);
    }
}


public class PostBuildHook
{
    [PostProcessBuild(0)]
    public static void OnPostprocessBuild(BuildTarget target, string cciPath)
    {
        if (!CiaForgeX.toolEnabled) return;

        string tempPath = Application.dataPath + "/Editor/CiaForgeX/Temp/";
        string toolsPath = Application.dataPath + "/Editor/CiaForgeX/Tools/";

        EditorUtility.DisplayProgressBar("CiaForgeX", "Creating temp directory", 0.1f);
        if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
        Directory.CreateDirectory(tempPath);

        EditorUtility.DisplayProgressBar("CiaForgeX", "Copying CCI file", 0.2f);
        File.Copy(cciPath, tempPath + "game.3ds");

        if (CiaForgeX.removeCCI)
        {
            EditorUtility.DisplayProgressBar("CiaForgeX", "Removing CCI file", 0.2f);
            File.Delete(cciPath);
        }

        if (CiaForgeX.removeXML)
        {
            EditorUtility.DisplayProgressBar("CiaForgeX", "Removing XML file", 0.2f);
            File.Delete(cciPath + ".xml");
        }

        EditorUtility.DisplayProgressBar("CiaForgeX", "Preparing necessary tools", 0.3f);
        File.Copy(toolsPath + "decrypt.bat", tempPath + "decrypt.bat");
        File.Copy(toolsPath + "makerom.exe", tempPath + "makerom.exe");
        File.Copy(toolsPath + "ctrtool.exe", tempPath + "ctrtool.exe");
        File.Copy(toolsPath + "decrypt.exe", tempPath + "decrypt.exe");

        EditorUtility.DisplayProgressBar("CiaForgeX", "Decrypting CCI file", 0.4f);
        Process decryption = new Process();
        decryption.StartInfo.FileName = "C:/Windows/System32/cmd.exe";
        decryption.StartInfo.Arguments = "/C  \""+ tempPath + "decrypt.bat\"";
        decryption.StartInfo.WorkingDirectory = tempPath;
        decryption.StartInfo.CreateNoWindow = true;
        decryption.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        decryption.Start();
        decryption.WaitForExit();

        File.Delete(tempPath + "game.3ds");
        File.Move(tempPath + "game-decrypted.3ds", tempPath + "game.3ds");

        if (CiaForgeX.toolMode == "Build CIA") BuildCIA(cciPath, toolsPath, tempPath);
        else ExtractModifyRebuildCIA(cciPath, toolsPath, tempPath);
    }


    static void BuildCIA(string cciPath, string toolsPath, string tempPath)
    {
        File.Copy(toolsPath + "3dsconv.exe", tempPath + "3dsconv.exe");

        EditorUtility.DisplayProgressBar("CiaForgeX", "Converting to CIA file", 0.7f);
        Process ciaBuild = new Process();
        ciaBuild.StartInfo.FileName = tempPath + "3dsconv.exe";
        ciaBuild.StartInfo.Arguments = "game.3ds";
        ciaBuild.StartInfo.WorkingDirectory = tempPath;
        ciaBuild.StartInfo.CreateNoWindow = true;
        ciaBuild.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        ciaBuild.Start();
        ciaBuild.WaitForExit();

        string outputDirectory = Path.GetDirectoryName(cciPath);
        string fileName = Path.GetFileNameWithoutExtension(cciPath);
        string outputFile = outputDirectory + "/" + fileName + ".cia";

        File.Copy(tempPath + "game.cia", outputFile, true);

        EditorUtility.DisplayProgressBar("CiaForgeX", "Cleaning up", 1f);
        Directory.Delete(tempPath, true);

        EditorUtility.ClearProgressBar();

        outputFile = outputFile.Replace('/', '\\');
        Process.Start("explorer.exe", "/select,\"" + outputFile + "\"");
    }


    static void ExtractModifyRebuildCIA(string cciPath, string toolsPath, string tempPath)
    {
        File.Copy(toolsPath + "3dstool.exe", tempPath + "3dstool.exe");
        File.Copy(toolsPath + "dummy.rsf", tempPath + "game.rsf");
        File.Copy(toolsPath + "ignore_3dstool.txt", tempPath + "ignore_3dstool.txt");
        File.Copy(toolsPath + "rsfgen.exe", tempPath + "rsfgen.exe");

        string manualPath = Directory.GetParent(Application.dataPath).FullName + "\\Temp\\StagingArea\\Manual\\Manual.cfa";
        bool hasManual = File.Exists(manualPath);
        if (hasManual) File.Copy(manualPath, tempPath + "Manual.cfa", true);

        EditorUtility.DisplayProgressBar("CiaForgeX", "Extracting rom", 0.5f);
        Process extractionProcess = new Process();
        extractionProcess.StartInfo.FileName = tempPath + "ctrtool.exe";
        extractionProcess.StartInfo.Arguments = "--exefsdir=exefs --romfsdir=romfs --exheader=exheader.bin game.3ds";
        extractionProcess.StartInfo.WorkingDirectory = tempPath;
        extractionProcess.StartInfo.UseShellExecute = true;
        extractionProcess.StartInfo.CreateNoWindow = true;
        extractionProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        extractionProcess.Start();
        extractionProcess.WaitForExit();

        EditorUtility.DisplayProgressBar("CiaForgeX", "Rebuilding romfs.bin", 0.6f);
        Process romfsRebuild = new Process();
        romfsRebuild.StartInfo.FileName = tempPath + "3dstool.exe";
        romfsRebuild.StartInfo.Arguments = "-cvtf romfs romfs.bin --romfs-dir romfs/";
        romfsRebuild.StartInfo.WorkingDirectory = tempPath;
        romfsRebuild.StartInfo.UseShellExecute = false;
        romfsRebuild.StartInfo.CreateNoWindow = true;
        romfsRebuild.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        romfsRebuild.Start();
        romfsRebuild.WaitForExit();

        EditorUtility.DisplayProgressBar("CiaForgeX", "Generating RSF file", 0.7f);
        Process rsfGeneration = new Process();
        rsfGeneration.StartInfo.FileName = tempPath + "rsfgen.exe";
        rsfGeneration.StartInfo.Arguments = "-r game.3ds -e exheader.bin -o game.rsf";
        rsfGeneration.StartInfo.WorkingDirectory = tempPath;
        rsfGeneration.StartInfo.UseShellExecute = false;
        rsfGeneration.StartInfo.CreateNoWindow = true;
        rsfGeneration.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        rsfGeneration.Start();
        rsfGeneration.WaitForExit();

        EditorUtility.DisplayProgressBar("CiaForgeX", "Modifying RSF file", 0.8f);
        string rsfPath = tempPath + "game.rsf";
        string systemMode = CiaForgeX.systemMode;
        string systemModeLine;
        switch (systemMode)
        {
            case "32MB":
                systemModeLine = "  SystemMode                    : 32MB # 64MB(Default)/96MB/80MB/72MB/32MB";
                break;

            case "72MB":
                systemModeLine = "  SystemMode                    : 72MB # 64MB(Default)/96MB/80MB/72MB/32MB";
                break;

            case "80MB":
                systemModeLine = "  SystemMode                    : 80MB # 64MB(Default)/96MB/80MB/72MB/32MB";
                break;

            case "96MB (NOT WORKING)":
                systemModeLine = "  SystemMode                    : 96MB # 64MB(Default)/96MB/80MB/72MB/32MB";
                break;

            default:
                systemModeLine = "  SystemMode                    : 64MB # 64MB(Default)/96MB/80MB/72MB/32MB";
                break;
        }

        List<string> rsfLines = new List<string>(File.ReadAllLines(rsfPath));

        int memoryTypeIndex = rsfLines.FindIndex(line => line.TrimStart().StartsWith("MemoryType"));
        rsfLines.RemoveAll(line => line.TrimStart().StartsWith("SystemMode"));
        if (memoryTypeIndex != -1) rsfLines.Insert(memoryTypeIndex + 1, systemModeLine);

        if (CiaForgeX.n3dsSystemMode != "Default")
        {
            string extModeLine = "";
            if (CiaForgeX.n3dsSystemMode == "124MB")
                extModeLine = "  SystemModeExt                    : 124MB";
            else if (CiaForgeX.n3dsSystemMode == "178MB")
                extModeLine = "  SystemModeExt                    : 178MB";

            if (!string.IsNullOrEmpty(extModeLine))
            {
                int memoryTypeIndex2 = rsfLines.FindIndex(line => line.TrimStart().StartsWith("MemoryType"));
                rsfLines.RemoveAll(line => line.TrimStart().StartsWith("SystemModeExt"));
                if (memoryTypeIndex2 != -1) rsfLines.Insert(memoryTypeIndex2 + 2, extModeLine);
            }
        }

        ReplaceConfigValue(rsfLines, "DisableDebug                  ", "false");
        ReplaceConfigValue(rsfLines, "EnableForceDebug              ", "false");
        ReplaceConfigValue(rsfLines, "CanWriteSharedPage            ", "true");
        ReplaceConfigValue(rsfLines, "CanUsePrivilegedPriority      ", "false");
        ReplaceConfigValue(rsfLines, "CanUseNonAlphabetAndNumber    ", "true");
        ReplaceConfigValue(rsfLines, "PermitMainFunctionArgument    ", "true");
        ReplaceConfigValue(rsfLines, "CanShareDeviceMemory          ", "true");
        ReplaceConfigValue(rsfLines, "RunnableOnSleep               ", "false");
        ReplaceConfigValue(rsfLines, "SpecialMemoryArrange          ", "true");

        File.WriteAllLines(rsfPath, rsfLines.ToArray());

        EditorUtility.DisplayProgressBar("CiaForgeX", "Rebuilding CXI/CIA file", 0.9f);
        Process cxiRebuild = new Process();
        cxiRebuild.StartInfo.FileName = tempPath + "makerom.exe";
        cxiRebuild.StartInfo.Arguments = "-f cxi -o game.cxi -rsf game.rsf -code exefs/code.bin -icon exefs/icon.bin -banner exefs/banner.bin -exheader exheader.bin -romfs romfs.bin";
        cxiRebuild.StartInfo.WorkingDirectory = tempPath;
        cxiRebuild.StartInfo.UseShellExecute = false;
        cxiRebuild.StartInfo.CreateNoWindow = true;
        cxiRebuild.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        cxiRebuild.Start();
        cxiRebuild.WaitForExit();

        Process ciaRebuild = new Process();
        ciaRebuild.StartInfo.FileName = tempPath + "makerom.exe";
        ciaRebuild.StartInfo.Arguments = "-f cia -o game.cia -content game.cxi:0:0";
        if (hasManual) ciaRebuild.StartInfo.Arguments += " -content Manual.cfa:1:1";
        ciaRebuild.StartInfo.WorkingDirectory = tempPath;
        ciaRebuild.StartInfo.UseShellExecute = false;
        ciaRebuild.StartInfo.CreateNoWindow = true;
        ciaRebuild.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        ciaRebuild.Start();
        ciaRebuild.WaitForExit();

        string outputDirectory = Path.GetDirectoryName(cciPath);
        string fileName = Path.GetFileNameWithoutExtension(cciPath);
        string outputFile = outputDirectory + "/" + fileName + ".cia";

        File.Copy(tempPath + "game.cia", outputFile, true);

        EditorUtility.DisplayProgressBar("CiaForgeX", "Cleaning up", 1f);
        Directory.Delete(tempPath, true);

        EditorUtility.ClearProgressBar();

        outputFile = outputFile.Replace('/', '\\');
        Process.Start("explorer.exe", "/select,\"" + outputFile + "\"");
    }

    static void ReplaceConfigValue(List<string> rsfLines, string key, string newValue)
    {
        int index = rsfLines.FindIndex(line => line.TrimStart().StartsWith(key));
        if (index != -1)
        {
            rsfLines[index] = "  " + key + ": " + newValue;
        }
    }
}