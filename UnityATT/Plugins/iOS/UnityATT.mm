//
//  UnityATT.m
//  UnityATT
//
//  Created by Vu Nguyen on 3/25/21.
//

#import <Foundation/Foundation.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

extern void UnitySendMessage(const char *obj, const char *method, const char *msg);

@interface UnityATT : NSObject;
@end

@implementation UnityATT;

+ (UnityATT*) instance {
    static UnityATT* helper = nil;
    static dispatch_once_t oneToken;
    dispatch_once(&oneToken, ^{
        helper = [[UnityATT alloc] init];
    });
    return helper;
}

- (void)RequestIDFA
{
    const char *targetObject = "UnityATTPlugin";
    const char *targetFunction = "OnRequestATTCallBack";
    
    if(@available(iOS 14, *))
    {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            switch (status) {
                case ATTrackingManagerAuthorizationStatusNotDetermined:
                    UnitySendMessage(targetObject, targetFunction, "0");
                    break;
                case ATTrackingManagerAuthorizationStatusRestricted:
                    UnitySendMessage(targetObject, targetFunction, "1");
                    break;
                case ATTrackingManagerAuthorizationStatusDenied:
                    UnitySendMessage(targetObject, targetFunction, "2");
                    break;
                case ATTrackingManagerAuthorizationStatusAuthorized:
                    UnitySendMessage(targetObject, targetFunction, "3");
                    break;
                default:
                    UnitySendMessage(targetObject, targetFunction, "4");
                    break;
            }
        }];
    }
    else
    {
        UnitySendMessage(targetObject, targetFunction, "4");
    }
}

-(int)ATTStatus
{
    int stt = 4;
    if(@available(iOS 14, *))
    {
        ATTrackingManagerAuthorizationStatus status = [ATTrackingManager trackingAuthorizationStatus];
        switch (status) {
            case ATTrackingManagerAuthorizationStatusNotDetermined:
                stt = 0;
                break;
            case ATTrackingManagerAuthorizationStatusRestricted:
                stt = 1;
                break;
            case ATTrackingManagerAuthorizationStatusDenied:
                stt = 2;
                break;
            case ATTrackingManagerAuthorizationStatusAuthorized:
                stt = 3;
                break;
            default:
                stt = 4;
                break;
        }
    }
    return stt;
}

extern "C"
{
    int getATTStatus(){
        int status = [[UnityATT instance] ATTStatus];
        return status;
    }
    
    void showATTRequest(){
        [[UnityATT instance] RequestIDFA];
    }
}

@end
