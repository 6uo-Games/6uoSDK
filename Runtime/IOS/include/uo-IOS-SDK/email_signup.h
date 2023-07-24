//
//  uo_IOS_SDK.h
//  uo-IOS-SDK
//
//  Created by Simon Lo on 7/13/23.
//

#import <Foundation/Foundation.h>

@interface email_signup : NSObject

+ (void)emailAddress:(const char *)emailAddress password:(const char *)password confirmPassword:(const char *)confirmPassword;

@end
