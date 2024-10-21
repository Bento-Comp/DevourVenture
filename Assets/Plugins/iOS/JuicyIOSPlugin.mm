#import <Foundation/Foundation.h>
#import <sys/utsname.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>
#import <StoreKit/StoreKit.h>
#import <StoreKit/SKAdNetwork.h>

@interface JuicyIOSPlugin : NSObject
{
    
}
@end

@implementation JuicyIOSPlugin
//Screen
//Pixels
+(double)getScreenPixelWidth
{
    return [UIScreen mainScreen].nativeBounds.size.width;
}

+(double)getScreenPixelHeight
{
    return [UIScreen mainScreen].nativeBounds.size.height;
}
//Points
+(double)getScreenPointWidth
{
    return [UIScreen mainScreen].bounds.size.width;
}

+(double)getScreenPointHeight
{
    return [UIScreen mainScreen].bounds.size.height;
}
//Scale
+(double)getScreenScale
{
    return [UIScreen mainScreen].scale;
}

+(double)getScreenNativeScale
{
    return [UIScreen mainScreen].nativeScale;
}

+(double)getScreenSafeAreaTop
{
    if (@available(iOS 11.0, *))
    {
        UIWindow *window = [UIApplication sharedApplication].keyWindow;
        return window.safeAreaInsets.top;
    }
    return 0;
}

+(double)getScreenSafeAreaBottom
{
    if (@available(iOS 11.0, *))
    {
        UIWindow *window = [UIApplication sharedApplication].keyWindow;
        return window.safeAreaInsets.bottom;
    }
    return 0;
}

//Device
+(char*)getDeviceModel
{
    struct utsname systemInfo;
    uname(&systemInfo);
    return cStringCopy([[NSString stringWithCString:systemInfo.machine encoding:NSUTF8StringEncoding]UTF8String]);
}

+(char*)getDeviceSystemVersion
{
    return cStringCopy([[UIDevice currentDevice].systemVersion UTF8String]);
}

char* cStringCopy(const char* string)
{
    if (string == NULL)
        return NULL;

    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);

    return res;
}

void callATTPopUp(const char* goN, const char* goM)
{
    //Cache the string otherwise they are fucked up in the callback
    char* goName = cStringCopy(goN);
    char* goMethod = cStringCopy(goM);

    if (@available(iOS 14, *))
    {
        //Display pop up and send callback to Unity when the users clicked on something
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status)
        {
            switch (status) {
                case ATTrackingManagerAuthorizationStatusAuthorized:
                    UnitySendMessage(goName,goMethod,"0");
                    break;
                case ATTrackingManagerAuthorizationStatusNotDetermined:
                    UnitySendMessage(goName,goMethod,"1");
                    break;
                case ATTrackingManagerAuthorizationStatusRestricted:
                    UnitySendMessage(goName,goMethod,"2");
                    break;
                default:
                case ATTrackingManagerAuthorizationStatusDenied:
                    UnitySendMessage(goName,goMethod,"3");
                    break;
            }
        }
        ];
    }
    //Before iOS14 is Ok no pop up
    else
    {
        UnitySendMessage(goName, goMethod, "0");
    }
}

int getATTStatus()
{
    if (@available(iOS 14, *))
    {
        ATTrackingManagerAuthorizationStatus status = [ATTrackingManager trackingAuthorizationStatus];
        switch (status)
        {
            case ATTrackingManagerAuthorizationStatusAuthorized:
                return 0;
            default:
            case ATTrackingManagerAuthorizationStatusNotDetermined:
                return 1;
            case ATTrackingManagerAuthorizationStatusRestricted:
                return 2;
            case ATTrackingManagerAuthorizationStatusDenied:
                return 3;
        }
    }
    else
    {
        if([[ASIdentifierManager sharedManager] isAdvertisingTrackingEnabled])
            return 0;
        else
            return 3;
    }
}

bool isIOS14OrAbove()
{
    if(@available(iOS 14, *))
    return true;
    else
        return false;
}

bool isIOS14Dot5OrAbove()
{
    if(@available(iOS 14.5, *))
    return true;
    else
        return false;
}

+(char*) getIDFA
{
    NSUUID *identifier = [[ASIdentifierManager sharedManager] advertisingIdentifier];
    return cStringCopy([[identifier UUIDString] UTF8String]);
}

//Rating
void displayRatingPopUp()
{
    if(@available(iOS 10.3, *))
        [SKStoreReviewController requestReview];
}


//Conversion Value
void updateConversionValue(int conversionValue)
{
    if (@available(iOS 14, *)){
        if (@available(iOS 16.1, *)) {
            [SKAdNetwork updatePostbackConversionValue:conversionValue
                                           coarseValue:SKAdNetworkCoarseConversionValueLow
                                            lockWindow:NO
                                     completionHandler:^(NSError *_Nullable error) {
                NSLog(@"An error occurred during completion: %a", error);
            }];
        }
        else {
            [SKAdNetwork updateConversionValue:conversionValue];
        }
    }
    
}
    
@end


//External functions
extern "C"
{
//Screen
//Pixels
    double _GetScreenPixelWidth()
    {
        return JuicyIOSPlugin.getScreenPixelWidth;
    }

    double _GetScreenPixelHeight()
    {
        return JuicyIOSPlugin.getScreenPixelHeight;
    }
//Points
    double _GetScreenPointWidth()
    {
        return JuicyIOSPlugin.getScreenPointWidth;
    }

    double _GetScreenPointHeight()
    {
        return JuicyIOSPlugin.getScreenPointHeight;
    }
//Scale
    double _GetScreenScale()
    {
        return JuicyIOSPlugin.getScreenScale;
    }

    double _GetScreenNativeScale()
    {
        return JuicyIOSPlugin.getScreenNativeScale;
    }
//Safe
    double _GetScreenSafeAreaTop()
    {
        return JuicyIOSPlugin.getScreenSafeAreaTop;
    }

    double _GetScreenSafeAreaBottom()
    {
        return JuicyIOSPlugin.getScreenSafeAreaBottom;
    }
//Device
    char* _GetDeviceModel()
    {
        return JuicyIOSPlugin.getDeviceModel;
    }

    char* _GetDeviceSystemVersion()
    {
        return JuicyIOSPlugin.getDeviceSystemVersion;
    }
//Privacy
    void _CallATTPopUp(char* goName, char* goMethod)
    {
        callATTPopUp(goName, goMethod);
    }

    int _GetATTStatus()
    {
        return getATTStatus();
    }

    bool _IsIOS14OrAbove()
    {
        return isIOS14OrAbove();
    }

    bool _IsIOS14Dot5OrAbove()
    {
        return isIOS14Dot5OrAbove();
    }

    char* _GetIDFA()
    {
        return JuicyIOSPlugin.getIDFA;
    }

//Rating
    void _ShowRatingPopUp()
    {
        displayRatingPopUp();
    }

//Conversion Value
    void _UpdateConversionValue(int conversionValue)
    {
        updateConversionValue(conversionValue);
    }
}



