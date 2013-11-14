using System;
using System.Runtime.InteropServices; // too many types. Let's include the whole namespace
 
using Int32Rect = System.Windows.Int32Rect;
using Freezable = System.Windows.Freezable;
using InteropImaging = System.Windows.Interop.Imaging;
using MarkupExtension = System.Windows.Markup.MarkupExtension;
using ImageSource = System.Windows.Media.ImageSource;
using BitmapSource = System.Windows.Media.Imaging.BitmapSource;
using TypeConverter = System.ComponentModel.TypeConverter;
using MediaImaging = System.Windows.Media.Imaging;
 
using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;
using ITypeDescriptorContext = System.ComponentModel.ITypeDescriptorContext;
using EnumConverter = System.ComponentModel.EnumConverter;
 
using CultureInfo = System.Globalization.CultureInfo;
 
namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
   public static class WindowsIcons {
      public static BitmapSource GetBitmapSource(StockIconIdentifier identifier) {
         return StockIcon.GetBitmapSource(identifier, 0);
      }
      public static BitmapSource DocumentNotAssociated {get {return GetBitmapSource(StockIconIdentifier.DocumentNotAssociated);}}
      public static BitmapSource DocumentAssociated {get {return GetBitmapSource(StockIconIdentifier.DocumentAssociated);}}
      public static BitmapSource Application {get {return GetBitmapSource(StockIconIdentifier.Application);}}
      public static BitmapSource Folder {get {return GetBitmapSource(StockIconIdentifier.Folder);}}
      public static BitmapSource FolderOpen {get {return GetBitmapSource(StockIconIdentifier.FolderOpen);}}
      public static BitmapSource Drive525 {get {return GetBitmapSource(StockIconIdentifier.Drive525);}}
      public static BitmapSource Drive35 {get {return GetBitmapSource(StockIconIdentifier.Drive35);}}
      public static BitmapSource DriveRemove {get {return GetBitmapSource(StockIconIdentifier.DriveRemove);}}
      public static BitmapSource DriveFixed {get {return GetBitmapSource(StockIconIdentifier.DriveFixed);}}
      public static BitmapSource DriveNetwork {get {return GetBitmapSource(StockIconIdentifier.DriveNetwork);}}
      public static BitmapSource DriveNetworkDisabled {get {return GetBitmapSource(StockIconIdentifier.DriveNetworkDisabled);}}
      public static BitmapSource DriveCD {get {return GetBitmapSource(StockIconIdentifier.DriveCD);}}
      public static BitmapSource DriveRAM {get {return GetBitmapSource(StockIconIdentifier.DriveRAM);}}
      public static BitmapSource World {get {return GetBitmapSource(StockIconIdentifier.World);}}
      public static BitmapSource Server {get {return GetBitmapSource(StockIconIdentifier.Server);}}
      public static BitmapSource Printer {get {return GetBitmapSource(StockIconIdentifier.Printer);}}
      public static BitmapSource MyNetwork {get {return GetBitmapSource(StockIconIdentifier.MyNetwork);}}
      public static BitmapSource Find {get {return GetBitmapSource(StockIconIdentifier.Find);}}
      public static BitmapSource Help {get {return GetBitmapSource(StockIconIdentifier.Help);}}
      public static BitmapSource Share {get {return GetBitmapSource(StockIconIdentifier.Share);}}
      public static BitmapSource Link {get {return GetBitmapSource(StockIconIdentifier.Link);}}
      public static BitmapSource SlowFile {get {return GetBitmapSource(StockIconIdentifier.SlowFile);}}
      public static BitmapSource Recycler {get {return GetBitmapSource(StockIconIdentifier.Recycler);}}
      public static BitmapSource RecyclerFull {get {return GetBitmapSource(StockIconIdentifier.RecyclerFull);}}
      public static BitmapSource MediaCDAudio {get {return GetBitmapSource(StockIconIdentifier.MediaCDAudio);}}
      public static BitmapSource Lock {get {return GetBitmapSource(StockIconIdentifier.Lock);}}
      public static BitmapSource AutoList {get {return GetBitmapSource(StockIconIdentifier.AutoList);}}
      public static BitmapSource PrinterNet {get {return GetBitmapSource(StockIconIdentifier.PrinterNet);}}
      public static BitmapSource ServerShare {get {return GetBitmapSource(StockIconIdentifier.ServerShare);}}
      public static BitmapSource PrinterFax {get {return GetBitmapSource(StockIconIdentifier.PrinterFax);}}
      public static BitmapSource PrinterFaxNet {get {return GetBitmapSource(StockIconIdentifier.PrinterFaxNet);}}
      public static BitmapSource PrinterFile {get {return GetBitmapSource(StockIconIdentifier.PrinterFile);}}
      public static BitmapSource Stack {get {return GetBitmapSource(StockIconIdentifier.Stack);}}
      public static BitmapSource MediaSVCD {get {return GetBitmapSource(StockIconIdentifier.MediaSVCD);}}
      public static BitmapSource StuffedFolder {get {return GetBitmapSource(StockIconIdentifier.StuffedFolder);}}
      public static BitmapSource DriveUnknown {get {return GetBitmapSource(StockIconIdentifier.DriveUnknown);}}
      public static BitmapSource DriveDVD {get {return GetBitmapSource(StockIconIdentifier.DriveDVD);}}
      public static BitmapSource MediaDVD {get {return GetBitmapSource(StockIconIdentifier.MediaDVD);}}
      public static BitmapSource MediaDVDRAM {get {return GetBitmapSource(StockIconIdentifier.MediaDVDRAM);}}
      public static BitmapSource MediaDVDRW {get {return GetBitmapSource(StockIconIdentifier.MediaDVDRW);}}
      public static BitmapSource MediaDVDR {get {return GetBitmapSource(StockIconIdentifier.MediaDVDR);}}
      public static BitmapSource MediaDVDROM {get {return GetBitmapSource(StockIconIdentifier.MediaDVDROM);}}
      public static BitmapSource MediaCDAudioPlus {get {return GetBitmapSource(StockIconIdentifier.MediaCDAudioPlus);}}
      public static BitmapSource MediaCDRW {get {return GetBitmapSource(StockIconIdentifier.MediaCDRW);}}
      public static BitmapSource MediaCDR {get {return GetBitmapSource(StockIconIdentifier.MediaCDR);}}
      public static BitmapSource MediaCDBurn {get {return GetBitmapSource(StockIconIdentifier.MediaCDBurn);}}
      public static BitmapSource MediaBlankCD {get {return GetBitmapSource(StockIconIdentifier.MediaBlankCD);}}
      public static BitmapSource MediaCDROM {get {return GetBitmapSource(StockIconIdentifier.MediaCDROM);}}
      public static BitmapSource AudioFiles {get {return GetBitmapSource(StockIconIdentifier.AudioFiles);}}
      public static BitmapSource ImageFiles {get {return GetBitmapSource(StockIconIdentifier.ImageFiles);}}
      public static BitmapSource VideoFiles {get {return GetBitmapSource(StockIconIdentifier.VideoFiles);}}
      public static BitmapSource MixedFiles {get {return GetBitmapSource(StockIconIdentifier.MixedFiles);}}
      public static BitmapSource FolderBack {get {return GetBitmapSource(StockIconIdentifier.FolderBack);}}
      public static BitmapSource FolderFront {get {return GetBitmapSource(StockIconIdentifier.FolderFront);}}
      public static BitmapSource Shield {get {return GetBitmapSource(StockIconIdentifier.Shield);}}
      public static BitmapSource Warning {get {return GetBitmapSource(StockIconIdentifier.Warning);}}
      public static BitmapSource Info {get {return GetBitmapSource(StockIconIdentifier.Info);}}
      public static BitmapSource Error {get {return GetBitmapSource(StockIconIdentifier.Error);}}
      public static BitmapSource Key {get {return GetBitmapSource(StockIconIdentifier.Key);}}
      public static BitmapSource Software {get {return GetBitmapSource(StockIconIdentifier.Software);}}
      public static BitmapSource Rename {get {return GetBitmapSource(StockIconIdentifier.Rename);}}
      public static BitmapSource Delete {get {return GetBitmapSource(StockIconIdentifier.Delete);}}
      public static BitmapSource MediaAudioDVD {get {return GetBitmapSource(StockIconIdentifier.MediaAudioDVD);}}
      public static BitmapSource MediaMovieDVD {get {return GetBitmapSource(StockIconIdentifier.MediaMovieDVD);}}
      public static BitmapSource MediaEnhancedCD {get {return GetBitmapSource(StockIconIdentifier.MediaEnhancedCD);}}
      public static BitmapSource MediaEnhancedDVD {get {return GetBitmapSource(StockIconIdentifier.MediaEnhancedDVD);}}
      public static BitmapSource MediaHDDVD {get {return GetBitmapSource(StockIconIdentifier.MediaHDDVD);}}
      public static BitmapSource MediaBluRay {get {return GetBitmapSource(StockIconIdentifier.MediaBluRay);}}
      public static BitmapSource MediaVCD {get {return GetBitmapSource(StockIconIdentifier.MediaVCD);}}
      public static BitmapSource MediaDVDPlusR {get {return GetBitmapSource(StockIconIdentifier.MediaDVDPlusR);}}
      public static BitmapSource MediaDVDPlusRW {get {return GetBitmapSource(StockIconIdentifier.MediaDVDPlusRW);}}
      public static BitmapSource DesktopPC {get {return GetBitmapSource(StockIconIdentifier.DesktopPC);}}
      public static BitmapSource MobilePC {get {return GetBitmapSource(StockIconIdentifier.MobilePC);}}
      public static BitmapSource Users {get {return GetBitmapSource(StockIconIdentifier.Users);}}
      public static BitmapSource MediaSmartMedia {get {return GetBitmapSource(StockIconIdentifier.MediaSmartMedia);}}
      public static BitmapSource MediaCompactFlash {get {return GetBitmapSource(StockIconIdentifier.MediaCompactFlash);}}
      public static BitmapSource DeviceCellPhone {get {return GetBitmapSource(StockIconIdentifier.DeviceCellPhone);}}
      public static BitmapSource DeviceCamera {get {return GetBitmapSource(StockIconIdentifier.DeviceCamera);}}
      public static BitmapSource DeviceVideoCamera {get {return GetBitmapSource(StockIconIdentifier.DeviceVideoCamera);}}
      public static BitmapSource DeviceAudioPlayer {get {return GetBitmapSource(StockIconIdentifier.DeviceAudioPlayer);}}
      public static BitmapSource NetworkConnect {get {return GetBitmapSource(StockIconIdentifier.NetworkConnect);}}
      public static BitmapSource Internet {get {return GetBitmapSource(StockIconIdentifier.Internet);}}
      public static BitmapSource ZipFile {get {return GetBitmapSource(StockIconIdentifier.ZipFile);}}
      public static BitmapSource Settings {get {return GetBitmapSource(StockIconIdentifier.Settings);}}
   }
 
   public class StockIcon : MarkupExtension {
      StockIconIdentifier _identifier;
      StockIconOptions _flags;
      BitmapSource _bitmapSource = null;
 
      public StockIcon() {
      }
 
      public StockIcon(StockIconIdentifier identifier) : this(identifier,0){
      }
 
      public StockIcon(StockIconIdentifier identifier, StockIconOptions flags) {
         Identifier = identifier;
         Selected = (flags & StockIconOptions.Selected) == StockIconOptions.Selected;
         LinkOverlay = (flags & StockIconOptions.LinkOverlay) == StockIconOptions.LinkOverlay;
         ShellSize = (flags & StockIconOptions.ShellSize) == StockIconOptions.ShellSize;
         Small = (flags & StockIconOptions.Small) == StockIconOptions.Small;
      }
 
      protected void Check() {
         if (_bitmapSource != null)
            throw new InvalidOperationException("The BitmapSource has already been created");
      }
 
      public bool Selected {
         get {return (_flags & StockIconOptions.Selected) == StockIconOptions.Selected;}
         set {
            Check();
            if (value)
               _flags |= StockIconOptions.Selected;
            else
               _flags &= ~StockIconOptions.Selected;
         }
      }
 
 
      public bool LinkOverlay {
         get {return (_flags & StockIconOptions.LinkOverlay) == StockIconOptions.LinkOverlay;}
         set {
            Check();
            if (value)
               _flags |= StockIconOptions.LinkOverlay;
            else
               _flags &= ~StockIconOptions.LinkOverlay;
         }
      }
 
      public bool ShellSize {
         get {return (_flags & StockIconOptions.ShellSize) == StockIconOptions.ShellSize;}
         set {
            Check();
            if (value)
               _flags |= StockIconOptions.ShellSize;
            else
               _flags &= ~StockIconOptions.ShellSize;
         }
      }
 
      public bool Small {
         get { return (_flags & StockIconOptions.Small) == StockIconOptions.Small; }
         set {
            Check();
            if (value)
               _flags |= StockIconOptions.Small;
            else
               _flags &= ~StockIconOptions.Small;
         }
      }
 
      public StockIconIdentifier Identifier {
         get { return _identifier; }
         set {
            Check();
            _identifier = value;
         }
      }
 
      public override Object ProvideValue(IServiceProvider serviceProvider) {
         return Bitmap;
      }
 
      public BitmapSource Bitmap {
         get {
            if (_bitmapSource == null) {
               _bitmapSource = GetBitmapSource(_identifier, _flags);
            }
            return _bitmapSource;
         }
      }
 
      protected internal static BitmapSource GetBitmapSource(StockIconIdentifier identifier, StockIconOptions flags) {
         BitmapSource bitmapSource = (BitmapSource) InteropHelper.MakeImage(identifier, StockIconOptions.Handle | flags);
         bitmapSource.Freeze();
         return bitmapSource;
      }
   }
 
   [Flags]
   public enum StockIconOptions : uint {
      Small = 0x000000001,       // Retrieve the small version of the icon, as specified by the SM_CXSMICON and SM_CYSMICON system metrics.
      ShellSize = 0x000000004,   // Retrieve the shell-sized icons rather than the sizes specified by the system metrics.
      Handle = 0x000000100,      // The hIcon member of the SHSTOCKICONINFO structure receives a handle to the specified icon.
      SystemIndex = 0x000004000, // The iSysImageImage member of the SHSTOCKICONINFO structure receives the index of the specified icon in the system imagelist.
      LinkOverlay = 0x000008000, // Add the link overlay to the file's icon.
      Selected = 0x000010000     // Blend the icon with the system highlight color.
   }
 
   public enum StockIconIdentifier : uint {
      DocumentNotAssociated = 0, // document (blank page), no associated program
      DocumentAssociated = 1,    // document with an associated program
      Application = 2,           // generic application with no custom icon
      Folder = 3,                // Folder (closed)
      FolderOpen = 4,            // Folder (open)
      Drive525 = 5,              // 5.25" floppy disk Drive
      Drive35 = 6,               // 3.5" floppy disk Drive
      DriveRemove = 7,           // removable Drive
      DriveFixed = 8,            // Fixed (hard disk) Drive
      DriveNetwork = 9,          // Network Drive
      DriveNetworkDisabled = 10, // disconnected Network Drive
      DriveCD = 11,              // CD Drive
      DriveRAM = 12,             // RAM disk Drive
      World = 13,                // entire Network
      Server = 15,               // a computer on the Network
      Printer = 16,              // printer
      MyNetwork = 17,            // My Network places
      Find = 22,                 // Find
      Help = 23,                 // Help
      Share = 28,                // overlay for shared items
      Link = 29,                 // overlay for shortcuts to items
      SlowFile = 30,             // overlay for slow items
      Recycler = 31,             // empty recycle bin
      RecyclerFull = 32,         // full recycle bin
      MediaCDAudio = 40,         // Audio CD Media
      Lock = 47,                 // Security lock
      AutoList = 49,             // AutoList
      PrinterNet = 50,           // Network printer
      ServerShare = 51,          // Server share
      PrinterFax = 52,           // Fax printer
      PrinterFaxNet = 53,        // Networked Fax Printer
      PrinterFile = 54,          // Print to File
      Stack = 55,                // Stack
      MediaSVCD = 56,            // SVCD Media
      StuffedFolder = 57,        // Folder containing other items
      DriveUnknown = 58,         // Unknown Drive
      DriveDVD = 59,             // DVD Drive
      MediaDVD = 60,             // DVD Media
      MediaDVDRAM = 61,          // DVD-RAM Media
      MediaDVDRW = 62,           // DVD-RW Media
      MediaDVDR = 63,            // DVD-R Media
      MediaDVDROM = 64,          // DVD-ROM Media
      MediaCDAudioPlus = 65,     // CD+ (Enhanced CD) Media
      MediaCDRW = 66,          // CD-RW Media
      MediaCDR = 67,           // CD-R Media
      MediaCDBurn = 68,        // Burning CD
      MediaBlankCD = 69,       // Blank CD Media
      MediaCDROM = 70,         // CD-ROM Media
      AudioFiles = 71,         // Audio Files
      ImageFiles = 72,         // Image Files
      VideoFiles = 73,         // Video Files
      MixedFiles = 74,         // Mixed Files
      FolderBack = 75,         // Folder back
      FolderFront = 76,        // Folder front
      Shield = 77,             // Security shield. Use for UAC prompts only.
      Warning = 78,            // Warning
      Info = 79,               // Informational
      Error = 80,              // Error
      Key = 81,                // Key / Secure
      Software = 82,           // Software
      Rename = 83,             // Rename
      Delete = 84,             // Delete
      MediaAudioDVD = 85,      // Audio DVD Media
      MediaMovieDVD = 86,      // Movie DVD Media
      MediaEnhancedCD = 87,    // Enhanced CD Media
      MediaEnhancedDVD = 88,   // Enhanced DVD Media
      MediaHDDVD = 89,         // HD-DVD Media
      MediaBluRay = 90,        // BluRay Media
      MediaVCD = 91,           // VCD Media
      MediaDVDPlusR = 92,      // DVD+R Media
      MediaDVDPlusRW = 93,     // DVD+RW Media
      DesktopPC = 94,          // desktop computer
      MobilePC = 95,           // mobile computer (laptop/notebook)
      Users = 96,              // users
      MediaSmartMedia = 97,    // Smart Media
      MediaCompactFlash = 98,  // Compact Flash
      DeviceCellPhone = 99,    // Cell phone
      DeviceCamera = 100,      // Camera
      DeviceVideoCamera = 101, // Video camera
      DeviceAudioPlayer = 102, // Audio player
      NetworkConnect = 103,    // Connect to Network
      Internet = 104,          // InterNet
      ZipFile = 105,           // ZIP File
      Settings = 106           // Settings
   }
 
   internal static class  InteropHelper {
      [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct StockIconInfo {
         internal UInt32 StuctureSize;
         internal IntPtr Handle;
         internal Int32 ImageIndex;
         internal Int32 Identifier;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
         internal string Path;
      }
 
      [DllImport("Shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
      internal static extern int SHGetStockIconInfo(StockIconIdentifier identifier, StockIconOptions flags, ref StockIconInfo info);
 
      [DllImport("User32.dll", SetLastError = true)]
      internal static extern bool DestroyIcon(IntPtr handle);
 
      internal static ImageSource MakeImage(StockIconIdentifier identifier, StockIconOptions flags) {
         IntPtr iconHandle = GetIcon(identifier, flags);
         ImageSource imageSource;
         try {
            imageSource = InteropImaging.CreateBitmapSourceFromHIcon(iconHandle, Int32Rect.Empty, null);
         } finally {
            DestroyIcon(iconHandle);
         }
         return imageSource;
      }
 
      internal static IntPtr GetIcon(StockIconIdentifier identifier, StockIconOptions flags) {
         StockIconInfo info = new StockIconInfo();
         info.StuctureSize = (UInt32) Marshal.SizeOf(typeof(StockIconInfo));
 
         int hResult = SHGetStockIconInfo( identifier, flags, ref info);
 
         if (hResult < 0)
            throw new COMException("SHGetStockIconInfo execution failure", hResult);
 
         return info.Handle;
      }
   }
}
