using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace Metal.Editor
{
    public static class InstallPackageHelper
    {
        private static Queue<string> PackagesToInstall = new();
        private static AddRequest CurrentRequest;

        private static event Action OnInstallCompleted;

        public static bool IsEdm4UInstalled()
        {
            return IsPackageInstalled(PackageConstant.EDM4U_PACKAGE_ID);
        }

        public static bool IsPackageInstalled(string packageId)
        {
            return Directory.Exists($"Packages/{packageId}");
        }

        public static void Install(string package, Action completed = null)
        {
            OnInstallCompleted = completed;
            PackagesToInstall = new Queue<string>();
            PackagesToInstall.Enqueue(package);
            ProcessNextPackage();
        }

        public static void Install(List<string> packages, Action completed = null)
        {
            OnInstallCompleted = completed;
            PackagesToInstall = new Queue<string>();
            foreach (var id in packages)
            {
                PackagesToInstall.Enqueue(id);
            }

            ProcessNextPackage();
        }

        private static void ProcessNextPackage()
        {
            if (PackagesToInstall.Count == 0)
            {
                MetalLog.Log("Tất cả package đã được cài đặt xong!");
                OnInstallCompleted?.Invoke();
                OnInstallCompleted = null;
                return;
            }

            string nextPackage = PackagesToInstall.Dequeue();
            MetalLog.Log($"Bắt đầu cài đặt {nextPackage}");
            CurrentRequest = Client.Add(nextPackage);

            EditorApplication.update += ProgressCheck;
        }

        private static void ProgressCheck()
        {
            if (!CurrentRequest.IsCompleted) return;

            EditorApplication.update -= ProgressCheck;

            if (CurrentRequest.Status == StatusCode.Success)
            {
                MetalLog.Log($"Đã cài xong {CurrentRequest.Result.packageId}");
            }
            else
            {
                MetalLog.LogError($"Lỗi khi cài {CurrentRequest.Error.message}");
            }

            ProcessNextPackage();
        }
    }
}
