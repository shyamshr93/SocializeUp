using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Graphics.Display;
using Windows.Services.Store;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace App6.Models
{
    public class UtilityData
    {
        public static LicenseInformation AppLicenseInformation { get; set; }
        public static SolidColorBrush SystemBrush { get; set; }
        public static SolidColorBrush ScrollBrush { get; set; }
        public static Uri ShareURL { get; set; }
        public static int LinksSetting { get; set; }
        public static bool isFluentDesign { get; set; }
        public static bool isbtHardwareBack { get; set; }
        public static bool newUpdate { get; set; }
        public static bool isFirstAppRun = false;
        public static bool isFacebookDark { get; set; }
        public static bool isTwitterDark { get; set; }
        public static bool isInstagramDark { get; set; }
        public static bool isTelegramDark { get; set; }
        public static bool isWhatsAppDark { get; set; }
        public static readonly string UpRemoveAds = "UpRemoveAds";
        public static readonly string RemoveAds = "RemoveAds";
        public static readonly string UpPin = "UpPin";
        public static readonly string PinIAP = "PinIAP";
        public static readonly string UnlockTelegram = "UnlockTelegram";
        public static readonly string UpTelegram = "UpTelegram";
        public static readonly string FacebookColor = "#4267B2", TwitterColor = "#138BDE", InstagramColor = "#8D3EBB", TelegramColor = "#5682A3", WhatsAppColor = "#009688";
        public static readonly string strChangelog = "Changelog";
        public static readonly string VersionNumber = "Version 6.7.8";
        public static OSVersion OperatingSystemVersion => SystemInformation.OperatingSystemVersion;
        public static StoreProductQueryResult addOnCollection;
        public static StoreProductQueryResult addOnsAssociatedStoreProducts;

        public static string getCss(string id)
        {
            string cssToApply = "";
            switch (id)
            {
                case "DFb":
                    cssToApply += @"
.uiScrollableArea .uiScrollableAreaWrap, _55ln, .__tw .jewelHeader, .__tw .jewelFooter a, ._5vsj._5vsj._5vsj, ._5v3q ._1dwg, ._4-u8, ._ipo, ._1g5v + ._4arz, ._4ar- ._3emk, .UFIReplyList, ._5vsj.UFIContainer .UFIReplyList .UFIRow, ._5vsj .UFIRow, ._1sdi, .fbNubButton, ._4mq3 .fbNubButton,  html .fbSettingsNavigation .divider, ._1yw, ._517h, ._59pe:focus, ._59pe:hover, ._2w3 > ._30f, ._3-a6 ._10la ._10lo, ._fyy, .__tw, ._2iwq div, ._605a ._1enb._1enb, ._3t3, ._t6b, ._41mp, ._4t2a, ._kj3, .uiBoxWhite, ._z6j , ._5i-7, ._30d, ._4-i0, ._4h7j, ._k7g, ._1eds, ._53ij, .uiMenu{ background: #171717 !important;}

._2s1x ._2s1y, ._33c, .uiButtonOverlay, .uiButtonOverlay:hover, #fbTimelineHeadline .profilePic, #fbTimelineHeadline .newProfilePic, ._6-6:hover, ._9rx.openToggler, ._9ry:hover, html ._9rw._54ne ._54nc, ._1_cb, ._6z- .uiSideNav .selectedItem > .item, ._6z- .uiSideNav .selectedItem > .subitem, ._53ij, ._3zsi, ._202, ._70l, .escapeHatchMinimal, ._3eqz._267a, ._6m2, ._3o_h::after, ._2yaa ._2yau::after, ._4z-w:hover, ._5y02:focus, ._5y02:hover, ._41nt {background: #1a1a1a;}

.fbTimelineStickyHeader .back, ._569t ._54ng, ._4lh ._2-d1, ._2tk, div._5a8u, ._4-u5, ._3od9 ._3odc {background: #1e1e1e;}

._m_1, .fbNub._50mz .fbNubFlyoutFooter, ._57d8, ._3m75 .selectedItem ._5afe::after, ._3m75 .sideNavItem:hover ._5afe::after, ._1cx1 ._ei_, ._4f7n, .loggedout_menubar_container, ._16ve, ._31qy , ._4mq3 ._55ln, .fbChatSidebarMessage,._5iwm ._58al, ._517h:hover, .jewelItemNew ._33e, ._6z- .uiSideNav .item:hover, ._6z- .uiSideNav .subitem:hover, ._202.selected {background: #292929;}

._531b, ._4oes,.UFIAddComment .UFIAddCommentInput._1osb, ._4mq3 ._55ln:focus, ._4mq3 ._55ln:hover, ._4mq3 li.selected ._55ln, ._1cx1 ._5g_r:hover ._m_1, ._1cx1 ._m_1:hover, ._55ln:focus, ._55ln:hover, li.selected ._55ln, ._585- {background: #333;}

._55ln, .highContrastSetting ._55y4 .sectionDragHandle,body, ._5iyx, .uiHeader h3, .uiHeader h4, ._5qtn ._5qtp, ._5vb_, ._5vb_ #contentCol, ._bui ._5afe, ._bui .subitem, ._5vsj._5vsj._5vsj,.fcb, ._52lp._59d- ._52lr, ._58mr, ._58mt, ._1y2l .author, ._1y2l a.messagesContent:hover .author, ._1y2l a.messagesContent:hover .author span, ._1y2l .subject, ._1y2l a.messagesContent:hover .subject, ._4mq3 .fbNubButton .label .count, .fbNubButton, ._5j0e a, ._3-9y, .uiSideNav .item, .uiSideNav .subitem, ._1y2l a.messagesContent, ._517h, ._59pe:focus, ._59pe:hover, .fbTimelineUnit, ._1sdd:hover, ._1sdd, .uiButtonText, .uiButton input, a._39g6, ._50f9, ._5pwr:hover ._5pws, ._47__ ._5pws, ._4-i0 ._52c9, ._6z- .uiSideNav .item, ._6z- .uiSideNav .subitem, ._5ewi ._5mtx .uiHeaderTitle, ._k7g, ._19bs, ._5108 .addFriendText, ._5108 .FollowLink, ._6m6 a, ._6590 ._5dwa ._38my, ._u9q ._5dwa ._38my, ._4nl3 ._5dwa ._38my, ._4-lu, ._2yap ._2yav, ._1cis ._3odd ._54nh, ._3od9 ._3_xs, .uiMenuItem .itemAnchor {color: white;}
                
._558b ._54nh, ._9jo li.navSubmenu a, .litestandClassicWelcomeBox.fbxWelcomeBox a, .litestandClassicWelcomeBox.fbxWelcomeBoxSmall a, ._3m75 .sideNavItem.sideNavItem.sideNavItem.sideNavItem ._5afe, ._m_1 ._2aha, ._3scn, ._2ezy, ._3f-h._3f-i, ._2wma, ._3v6c span._38my, ._31q_ , ._4mq3 .fbNubButton, .fbSettingsList .fbSettingsListItemLabel, ._3c_:hover ._3sz, ._3s- ._3sz, ._33hy ._554b:hover, ._4o52:hover ._3sz, ._3c- a, ._5_xt, ._33e, ._2fvv, .uiHeader h2, ._2jyf{color : white;}
                
#fbTimelineHeadline .profilePic, #fbTimelineHeadline .newProfilePic, html ._5k35 ._5k3a, ._6z- .uiSideNav .selectedItem > .item, ._6z- .uiSideNav .selectedItem > .subitem , ._3zsi, ._70l, ._1nv5 ._11kf, ._3o_h::after, ._2yaa ._2yau::after{border-color: #1a1a1a;}

._a7s, ._fgm ._1t6k + ._37uu ._610i._a7s._20h6, ._22jw._37uu ._610i._a7s._20h6, ._s_6._37uu ._610i._a7s._20h6, .uiCollapsedList ._20h6._a7s._610i._a7s._610i, ._14b9 ._r95, ._14b9 ._gx6, ._52bu ._3ow- , ._53ip ._53ij, .uiMenuSeparator {border-color: #333;}

._1y2l .jewelContent a.messagesContent, .__tw .jewelHeader, .__tw, ._4oes, #contentCurve, ._3nzk li, ._2jq5, ._5vsj .UFIAddComment .UFIAddCommentInput, ._5vsj .UFIAddComment .UFIAddCommentInput._1osc,._3nzp .uiScrollableArea.contentAfter, ._4kt > li, .pagelet ._2w2d, .pagelet ._1pp_, .uiHeaderSection, .uiSideHeader, ._5vsj.UFIContainer .UFIReplyList ._4oep,  ._6-6 , ._6_7, ._5pcm, ._5i_t, ._4ks > li, ._517h, ._59pe:focus, ._59pe:hover, ._46ye, ._2w3 > ._30f, ._5_xt, ._33c, ._fyy, ._3y6_, .fbTimelineStickyHeader .back, ._605a ._1enb._1enb, ._22jv, .uiButtonGroup .uiButtonGroupItem, ._3nzk .objectListItem, div._9rv ._54ng, ._4ms4, ._3t3, ._1_cb, .uiHeaderNav, .uiBoxWhite, ._3-z, ._30d ._3-z ~ ._3-z, .uiHeaderTopAndBottomBorder, ._5ewi ._5mtx, ._5lnf, ._4-u2 > ._4-u3, .hasLeftCol #mainContainer, .fbSettingsListBorderTop, .fbSettingsListBorderBottom, ._5tlx, ._2xbx, ._3x-2 ._5r69._1hvl, html ._4__g, ._rzn {border-color: #333;}
                
._5pr2.fbChatSidebar, .__tw, ._ei_, ._4d6h, ._4-u2, ._a7s, ._5vsj .UFIRow._4204, ._5vsj .UFIShareRow, ._5vsj .UFILikeSentence, ._5vsj ._48pi, ._3ubp, ._3cz, ._41mp, ._kj3, ._z6j, ._5ewg ._5m65, ._30d , .uiMenuItem .itemAnchor {border-color: #171717;}
                
 ._3m75 .selectedItem ._5afe::after, ._3m75 .sideNavItem:hover ._5afe::after, ._2dck, ._1cx1 ._ei_, ._4f7n, ._2yq._2ltu ._4-u2::before, ._2ltu ._4-u2, ._1qp6 .fbProfileBrowserListContainer .fbProfileBrowserListItem, html ._262m ._698, ._3-a6._1blz ._fmi, ._3-a6._1blz._5vsj .UFIRow.UFIAddComment._4204, ._2yq ._4-u2::before {border-color: #1e1e1e;}
               
._2s1x ._2s1y {border-bottom: 1px solid #1a1a1a;}

._fmi {border: 1px solid #171717;}

._3ubp, ._sg1, ._558b ._54ng {background: #171717;}

._5yk1, ._fmi, ._4d3w._u77 .fbPhotosSnowboxFeedbackInput .UFIRow, ._4d3w._u77 .fbPhotosSnowboxFeedbackInput {background: #292929; }

.__tw div.jewelHeader,._558b ._54ak, .uiSideNav .item, .uiSideNav .subitem, ._5v3q ._5g-l {border-bottom: 1px solid #333;}

._1y2l .preview, ._1y2l a.messagesContent:hover .preview, ._3f-h, html ._4lh ._1zw6, ._3-9y {color: #e9e9e9;}

.__tw .jewelFooter a,._16ve ._16vf, .fbChatSidebarMessage, ._4xdg,._16ve {border-top: 1px solid #333;}

.hasLeftCol #contentCol {border-left: 1px solid #333;}

._558b ._54nc {border: solid #171717;}

._558b ._54ng, ._585-, html ._6-d .fbTimelineSection, ._4mq3 .fbNubButton, .fbNub._50mz .fbNubButton, ._5va1, ._1yw, .uiBoxGray {border: 1px solid #333;}

._3sco, ._c24 { color: #c2c2c2;}

#pagelet_ego_pane, ._26z1 {display: none;}

._2yq #globalContainer {1012px !important; background: #1e1e1e;}

._1y2l li.jewelItemNew a.messagesContent, ._3cz, ._3-a6._1blz .UFICommentActorAndBody, ._o0o ._3-a6._1blz .UFICommentActorAndBody {background-color: #1e1e1e;}

html ._4lh, ._4lh .fbTimelineTimePeriod, ._4lh .fbTimelineSectionExpandPager .uiMorePagerLoader, ._4lh .fbTimelineCapsule li:last-child.anchorUnit, ._4lh .fbTimelineSectionLoading .loadingIndicator {background: #292929;}

._3u6q ._2s1y {background: #1a1a1a !important;}

._5vb_.hasLeftCol #mainContainer ,body, ._5vb_, ._5vb_ #contentCol, .fbIndex .gradient, .fbNub._50mz .fbNubFlyoutBody, ._585- ._4w98, .fbTimelineSection, div._4-i2, #contentCol, #leftCol, ._t, .uiBoxGray, ._1qp6 .fbProfileBrowserListContainer .fbProfileBrowserListItem, html ._262m ._698 {background: #1e1e1e;}

._16ve {margin: 0px;}

._4_dr._20h5 {border-color: #292929;}

._568z {padding-right: 27px;}

h1, h2, h3, h4, h5, h6 {color: white;}

html ._4lh, ._4lh .fbTimelineTimePeriod, ._4lh .fbTimelineSectionExpandPager .uiMorePagerLoader, ._4lh .fbTimelineSectionLoading .loadingIndicator {background: #1e1e1e;}

#globalContainer {background: transparent;}

._3m75 .selectedItem ._55yu > a, ._3m75 .sideNavItem:hover ._55yu > a, ._3m75 .selectedItem .uiSideNavEditButton a, ._3m75 .sideNavItem:hover .uiSideNavEditButton a {background-color: #282828; border-color: #282828;}
";

                    break;

                case "MFb":
                    cssToApply += @" 
 ._2v9s, .jewel .flyout .more, ._4qax, ._2cpp, .touch ._56bg, ._4_xl, .acg, ._2k51 {background: #292929;}

._59te.popoverOpen, ._59te.isActive, ._55wo, ._3jce, ._3bg5 ._52x1, ._1p70, ._u2c, ._5c0e, .tlBody, #timelineBody, .timelineX, .timeline .feed, .timeline .tlPrelude, .timeline .tlFeedPlaceholder, .touch ._51v6, .touch ._u2c, ._1ih_, .touch ._5c9u, .touch ._5ca9, .touch button._5c9u, ._333v, ._2b03, ._2a_g._2a_g, .jewel ._4l9b, .jewel .flyout , ._5-lw, .touch ._uoq, .touch ._37fb, ._3x1q ._1553._1553, ._vqv, ._1p70, ._2rgt {background: #171717;}

._2nx_._3jbj, ._9_7._a58 {background: #171717 !important;}

.acw, ._59e9, ._5c4t ._1g05, .jewel .flyout, ._3bg5 ._53_-, .aclb, .touch ._1oby ._5c9u, ._10c_ ._2jl2, .touch ._4_xl ._5c9u, ._2b06, .jewel .flyout .more, ._5-lx, .touch ._4_d0, ._52z5._1uh1, ._5oxw, html ._5t28, .touch ._3f50, ._953#root, .touch ._2di- , ._24e1, .touch ._u2c, ._1ih_ {background: #1e1e1e;}

._1p78 i {border-color: #1e1e1e;}

._52z5, ._z-w , ._13e_, ._1t4h, ._234-, ._3tl8, ._65wz, .touch ._50pc, ._hdn._hdn, ._14v5._14v5 ._14v8, ._4edl, .touch ._5whq, .touch .btnD, .touch .btnI, .touch ._45fu ._18qg ._1_ac , ._94v, ._42b6._42b6._42b6{background: #1a1a1a;}

._2zh4, ._15ks, ._1g05, ._d4i, .touch ._3o0d::after {background: #333;}

._52jb, .fcg, ._4qau {color : #e5e5e5;}

body, ._2zh4, ._15ks, ._5j35, ._1s79._1s79, .jewel .flyout .messages-flyout-item .thread-title, .touch textarea._5whq, ._391s, .touch ._3bg5 ._53_- ._5lut, .touch ._3bg5 ._53_- ._53__, ._4qas, ._3jce._26wa, .touch ._56bg, .blueName, .touch ._4mo, .touch ._5c9u, .touch ._5ca9, .touch button._5c9u, ._2a_i a, ._2a_i._2a_i a, ._333v ._108_, ._5pxa ._5pxc a , ._52jb, ._5-lx, ._io1, ._uwx.mentions-input, ._5c4t ._1g06, .touch ._5lm6, ._hdn._hdn, ._52ja, .touch ._37fd .inlineComposerButtonNew .composerLinkText, .touch ._37fd .inlineComposerButton.redesignStatusButton .composerLinkText, .touch ._37fd .inlineComposerButton .composerLinkText, .touch ._2ya3, .fcb, .touch ._1zq- .title , .touch a, ._29zn .__rz a, ._24e1, ._1p79{color : white;}

._2rgt {color: white !important; }

._3qet .fcg, ._52j9, ._3z10, ._2rbw, .touch ._2rbw ._41nw, ._5zgx {color: #c2c2c2;}

._15n_, .touch ._52t1, ._5ecn {background: #2b2b2b;}

._43mg, ._5t3b, .touch ._qw9 {display: none;}

._15n_, ._5j35::after, ._15ny::after, .acw, ._44qk, .touch ._52t1, ._5ecn, ._uww,.jewel .flyout, ._3bg5 ._52x6, ._4qax, ._2cpp, .aclb, ._1ha, ._10c_ ._2jl2, .touch ._1aj4, .touch ._1n8h ._1oby, ._45kb > div, .async_composer._2a_g :first-child._10pt + ._5ru3 , .jewel .flyout, .mRequestMoreItem, .jewel ._3wjp, ._1e8h, ._z-w , ._13e_, ._io2, ._1t4h, ._5_50, ._45kb > div, .touch ._4_d0 ._4_d1, ._2t39, .touch ._37fd, .touch ._37fd .inlineComposerButton, .touch ._37fd ._2jgw, ._1tzi, ._8he, ._2lut, ._3q6s, .touch .btnD, .touch .btnC.bgb, .touch .btnI, ._5whq, ._5p-6, ._1q6v, ._1hb::before, .touch ._ydx ._195r::before, ._4o58::after, .touch ._2s21 > *::after, ._1ih_ {border-color: #333;}

._129-, ._3x1q ._94v {border-color: #1a1a1a;}

._3-8w {margin: 0px;}

._2ip_ ._2zh4::before, ._2ip_ ._15kk::before, ._2ip_ ._15kk + ._4u3j::before, ._4e8n, ._uww, ._1ih_ {background: #333 !important;}

._55wo {border-color: #333 !important;}

._15ks ._15kl::before {border-left: 1px solid #333;}

._3bg5 ._52x1, ._gui {border-bottom: 1px solid #333;}

._3bg5 ._52x1, ._gui, ._5f99 {border-top: 1px solid #333;}

._4e8n, ._d4i {border: 1px solid #333;}

#header {position: fixed; width:100%; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;}

.flyout {max-height:15px; overflow-y:scroll;}

.chatHighlight {-webkit-animation-duration: 0s;}

.touch ._5c9u, .touch ._5ca9, .touch button._5c9u, ._513x, .touch ._ydx ._195r::before {background-image: none;}

.touch ._4_d0 ._4_d1, .touch ._5c9u, .touch ._5ca9, .touch button._5c9u, ._1hc {text-shadow: none;}

.touch .btnD, .touch .btnI {box-shadow: none;}

._14v5._14v5 ._14v8 {box-shadow: 1px 1px 3px 0px #333;}


";
                    break;

                case "DTwit":
                    cssToApply += @"

.module .flex-module, .dropdown-menu, .RetweetDialog-footer, .ProfileNav-item--userActions, .ProfileCanopy-navBar, .ProfileHeading-content, .DashboardProfileCard, .stream-end-inner, .new-tweets-bar, .account, .tweet, .app, .Trends,  .permalink-tweet, .stream-item-activity-notification, .conversation-module .stream-item, .conversation-module .original-tweet-item, .conversation-module .conversation-header, .conversation-module .missing-tweets-bar, .permalink-tweet:hover, .tweet:hover, .ThreadedConversation-viewOther, .ThreadedConversation-showMore, .inline-reply-tweetbox, .permalink, .login-responsive .page-canvas, .DashboardProfileCard-avatarLink, .ProfilePage-timelineHeader { background: #171717 !important;}

 .DMActivity-header, .modal-header,  .QuoteTweet, .top-timeline-tweetbox .tweet-user,.home-tweet-box, .RetweetDialog-commentBox, .WebToast-box--altColor, .content-main .conversations-enabled .expansion-container .inline-reply-tweetbox, .ThreadedConversation-moreReplies {background: #292929;}

 .global-nav-inner {background: #1a1a1a;}

.Trends, .hovercard, .global-nav .search-input, .module .list-link:hover,input, textarea, .t1-select, :not(.highlighted).ActivityItem .QuoteTweet--slim:hover {background: #333;}

.AdaptiveFiltersBar, .AdaptiveRelatedSearches, .SidebarFilterModule, .WhoToFollow, .ProfileCard, .content-inner, .module .list-link:hover, .module .active .list-link,:not(.no-header-background-module).stream-item, .hovercard .profile-social-proof {background: #171717;}

.dropdown-divider, .Trends, .global-nav .search-input, .content-header, .content-no-header, .TwitterCard-container, .QuoteTweet:hover, .QuoteTweet:focus, .QuoteTweet:active {border-color: #333;}

body, .QuoteTweet .tweet-content, .QuoteTweet-text a, .QuoteTweet-text a:hover, .QuoteTweet-text a:focus, .QuoteTweet-text a:active, .QuoteTweet-text .pretty-link b, .QuoteTweet-text .pretty-link s, .QuoteTweet-text .pretty-link:hover b, .QuoteTweet-text .pretty-link:hover s, .QuoteTweet-text .pretty-link:focus b, .QuoteTweet-text .pretty-link:focus s, .QuoteTweet-text .pretty-link:active b, .QuoteTweet-text .pretty-link:active s, .ProfileCard-userFields, .ProfileHeaderCard-locationText, .ProfileHeaderCard-joinDateText, .ProfileHeaderCard-vineProfileText, .ProfileHeaderCard-birthdateText, .tweet .js-tweet-text, .fullname, .QuoteTweet-fullname, .nav > li, .t1-legend, .typeahead a, .typeahead .fullname, .signin .remember, .flex-module-header h3, .AdaptiveSearchPage-moduleTitle, .ProfileHeaderCard-bio, .WhoToFollow-title {color: white;}

.content-main .stream-items > :not(.open):last-child.stream-item > .expansion-container > li .inline-reply-tweetbox, .content-main .stream-items > :not(.open):last-child.stream-item > .expansion-container > li .view-more-container, .modal-header, .new-tweets-bar, .content-main .expansion-container > .original-tweet-container, .content-main .expansion-container > li .tweet, :not(.no-header-background-module).stream-item, .tweet, .Trends, .tweet .stats, .permalink .inline-reply-tweetbox, .permalink.has-replies .inline-reply-tweetbox, .top-timeline-tweetbox .timeline-tweet-box, .QuoteTweet {border-color: #333;}

.module .flex-module, .ProfileCanopy-navBar,.DashboardProfileCard-bg, .DashboardProfileCard, .ProfileHeading-content, .DashboardProfileCard-avatarImage {border-color: #171717;}

.Footer .Footer-adsModule, .login-responsive .mobile, .global-nav--newLoggedOut #global-actions>li>a {display: none;}

.wtf-module.has-content {display: block;}

.AppContent, body,  body.three-col .wrapper, .DMActivity-body,.DMInboxItem, .NotificationsHeadingContent, .content-header .header-inner, .content-no-header .no-header-inner, .module .list-link {background: #1e1e1e;}

.permalink .in-reply-to .tweet, .permalink .replies-to .tweet, .DMInboxItem,.DMActivity-header, .stream-item-activity-notification, .NotificationsHeadingContent, .content-header .header-inner, .content-no-header .no-header-inner, .content-inner.no-stream-end, .ThreadedConversation, .ThreadedConversation-moreReplies::after, .ActivityItem {border-bottom: 1px solid #333;}

.AdaptiveSearchTimeline .stream :first-child.stream-item, hr, .module li:first-child .list-link {border-top: 1px solid #333;}

.content-inner, .permalink .stats .avatar-row a:first-child {border-left: 1px solid #333;}

.content-inner {border-right: 1px solid #333;}

.AdaptiveRelatedSearches, .SidebarFilterModule, .WhoToFollow, .ProfileCard, .module .list-link,input, textarea, div[contenteditable], .t1-select {border: 1px solid #333;}

.DMInboxItem:hover {background: #333 !important;}

.global-nav .search-input, .stream-item-activity-line-notification .fullname, .ProfileHeading-toggleItem.is-active, .ProfileHeading-toggleItem.is-active:hover, .ProfileHeading-toggleItem.is-active:focus, .SearchExtrasDropdown .dropdown-menu li > .SearchExtrasDropdown-target, .wrapper-settings .header-inner h2, .module .list-link, .module .active .list-link, .t1-label, .GalleryTweet a.with-icn:not(:hover) .Icon, .GalleryTweet .account-group .fullname, .GalleryTweet .account-group:hover .fullname, .ProfileSidebar .flex-module-header h3 {color: white;}

input:focus, textarea:focus, .DashUserDropdown.dropdown-menu li > a, .DashUserDropdown.dropdown-menu button {color: white;}

.AdaptiveFiltersBar-target {color: #c2c2c2;}

.login-responsive .page-canvas {box-shadow: none;}

.ProfilePage-timelineHeader {border: none;}



";
                    break;

                case "MTwit":
                    cssToApply += @"
.rn-1w6e6rj {display: inline-flex;}

.r-14lw9ot, body {background: #1a1a1a;}

.r-e84r5y, .r-vrwoeq {background: #1f1f1f;}

.rn-vrwoeq, .rn-1ou8vq3, .r-1ou8vq3, .r-vqp9x9 {background: #292929;}

.r-hkyrab {color: white;}

.rn-my5ep6, .rn-1gkumvb, .rn-1gkumvb, .r-1gkumvb, .r-uvzvve {border-color: #333;}

.rn-1ncdyz2, .rn-uvzvve, .rn-13b02wl, .rn-19ixy43, .rn-1eqmklk, .rn-3pxcvb, .rn-1iih0mj, .rn-1f0fu4f, .r-11mg6pl {border-color: #1a1a1a;}
                   
.r-1u4rsef {background : #1e1e1e;}

body, .fullname, ._3ZSf8YGw, ._1HXcreMa, ._3facEWb0, ._3facEWb0:active, ._3facEWb0:focus, ._3facEWb0:hover, .rn-q9ob72 {color: white;}

.r-9x6qib, .r-1tlfku8, .r-9x6qib,.r-9cbz99 {border-color : #1e1e1e;}

.tweet-detail .main-tweet-actions td, .tweet-gallery .main-tweet-actions td, .tweet-detail .main-tweet .last-section, .tweet-gallery .main-tweet .last-section, .tweet-detail .tweet-stats .stat, .tweet-gallery .tweet-stats .stat {border-color: #171717;}

.r-my5ep6 {border-color: #333;}

.rn-lfc6mk, .rn-1ykcu4d, .rn-1y2kpd, .rn-jebswk {border-color: #1a1a1a;}

._1d_6kzhv, ._1L-Rr7sz, .FsR6j-G7, ._1YVNMg8K ._29kyA7wX,._3cKAE7hX, ._1_FMKzvm, .yY8bJa97, ._3rF86cXC, ._3THoPquW,._2lr8ULQi ,._2WiB3NTn, .rn-mikf4x, ._2gQ_TiHv, .rn-12xski5 {background: #292929;}

.LqlkGmch,  ._24u5-vsm,._3VY1UWs3, .rn-44z8sh, ._1EEmDxFW, ._12kgJrOL, ._20OFrCPI:active, ._20OFrCPI:focus, ._20OFrCPI:hover{background: #1a1a1a;}

._1hmq-Mkf, ._1Zp5zVT9, ._34Ymm628 {color: #c2c2c2;}

._1t1yuVSm, ._3ZNFEy6P, ._3ZNFEy6P:active, ._3ZNFEy6P:focus, ._3ZNFEy6P:hover, ._3qQLFoYV, ._3qQLFoYV:active, ._3qQLFoYV:focus, ._3qQLFoYV:hover, .rn-6gldlz {color: #e9e9e9;}

._15-p_S0N, ._1wmatVSh, ._2vY0UT8z:active, .ktZMpANQ, ._1lW9BO8P, .FsR6j-G7:active, .FsR6j-G7:focus, .FsR6j-G7:hover, ._1y-7iMRs:active, ._1y-7iMRs:focus, ._1y-7iMRs:hover, ._38myriBx, ._38myriBx:active, ._38myriBx:focus, ._38myriBx:hover, .ys1Birxa, ._1IYhfl3z:active, ._3bf6snf7 , ._3XRskxP3:active,._1fsL8NFb:hover, .rn-1uhmdza {background: #333;}

._15-p_S0N, ._3QPrjhX2, ._3Ko_vUO1, .MmJh82_T {border: 1px solid #333;}

.pQ0LYqFV, ._22uh2-tW, ._33HzS5QJ, .zeo-m5c9, :last-child._1_FMKzvm {border-top: 1px solid #333;}

._1XHWlb5c, ._2OhV6WSs ._1eF_MiFx ._2sSKjr8W, ._1_vKaW5Q, .FsR6j-G7, ._3fv5_7QL, ._1Oxj60h6, ._2nhq3o_B, .FaFY-n1u,._1gq1kIcW, ._1_FMKzvm, ._1uaC9Qwe, ._2Axq_bxo, ._3RF62ZvD, .UYYwqQ_c, .FaFY-n1u, ._2_6WZWUw {border-bottom: 1px solid #333;}

.lIgYF7yz {border: 1px solid #1e1e1e;}

._3vxCixKF {border: 1px solid #171717;}

._3jSuHAjn, body, ._3Gyh2OCT,  .wErp7S_5, ._1HXcreMa  {color: white;}

.LqlkGmch {box-shadow: 0px 0px 4px #292929;}

body {background-color: #1a1a1a !important;}



";
                    break;

                case "Tele":
                    cssToApply += @"
.modal-content, .im_dialogs_search_field, .tg_head_split, .im_history_select_active .im_message_outer_wrap:hover, .tg_page_head .navbar-inverse, .im_message_body, .im_message_out .im_message_body {background: #292929;}

.im_dialogs_search_field:active, .im_dialogs_search_field:focus, .im_dialogs_scrollable_wrap .active a.im_dialog, .im_message_selected .im_message_outer_wrap, :empty.composer_rich_textarea, .composer_rich_textarea:active, .composer_rich_textarea:focus, .dropdown-menu .divider {background: #333;}

.im_dialogs_scrollable_wrap a.im_dialog:hover, .im_dialogs_scrollable_wrap a.im_dialog_selected, .im_dialogs_scrollable_wrap .active a.im_dialog, .im_dialogs_scrollable_wrap .active a.im_dialog:hover, .im_dialogs_scrollable_wrap .active a.im_dialog_selected, .dropdown-menu > li > a:focus, .dropdown-menu > li > a:hover, .tg_head_peer_dropdown .dropdown-menu > li > a:hover {background: #333;}

body, .im_dialog_peer, .dropdown-menu > li > a, a.mobile_modal_action, span.mobile_modal_action, h1, h2, h3, h4, h5, .form-control, .dropdown-menu > li > a:focus, .dropdown-menu > li > a:hover, a.tg_checkbox {color: white;}

body, .im_page_wrap, .mobile_modal.modal {background: #1e1e1e;}

.dropdown-menu {background: #171717;}

.form-control.no_outline:focus, .im_edit_panel_border, .im_dialogs_col_wrap, :empty.composer_rich_textarea {border-color: #333;}

.im_dialogs_search_field {border-color: #292929;}

.im_page_wrap {border-color: #1e1e1e;}

.im_dialogs_scrollable_wrap a.im_dialog {border-top: 1px solid #333;}

.footer_wrap, .dropdown-menu {border: 1px solid #333;}

.mobile_modal_section, .mobile_modal_action_wrap,.md_modal_iconed_section_wrap {border-bottom: 1px solid #333;}
";
                    break;

                case "Insta":
                    cssToApply += @"
.APQi1, ._8Rna9, .SkY6J, ._5pVk-, .ZsSMR,.VILGp, .N8xpH {display: none;}

 .gr27e, .pbNvD, ._6oveC, .vtWDf, .hUQsm, .ltEKP .QBXjJ, .t0fbY, .o9-it, .BvMHM, .drKGC, .t48Bo, .YHaCL, .HYpXt, .j6cq2, ._09ncq, .uzKWK, .PdTAI, ._7XkEo, .uUIL6, ._914pk, :not(.Fzijm).bR_3v, .DPiy6, ._1Yr2-, .eShHj { background: #171717 ;}

._2hvTZ, ._9GP1n, ._lz6s, .YQf7h, .fuQUr:hover, .JvDyy, ._9ezyW, .E3X2T, .AcVnq, .KQA-S, ._1MzIy, ._3MPWk, ._8Rm4L, .zMhqu, .zGtbP {background: #1a1a1a !important;}

._3Laht,  .o64aR, .Di7vw, .XTCLo, ._2dbep, .XAtZx, ._4Kbb_, .NP414, .NP414::before, .mAUDl,.abaSk, ._6xe7A, .wwxN2, .zwZPW, .tHaIX, ._41KYi, .jLuN9, .HYpXt.pbNvD, .N9abW, ._9ezyW::before {background: #1e1e1e !important;}

.KGiwt {background: #101010;}

 ._5mToa, .sH9wk, .aOOlW, .-fzfL, .fx7hk, ._5mToa, .JyscU .UE9AK, .JyscU .Slqrh, .PUHRj::after, ._1xe_U, .u_1x6, .NroHT, ._4Kbb_, .NP414, .JLJ-B, .p7vTm, .wW1cu , .zOJg- , .psMFr, .I6l20, :not(:last-child).bt7LU, .D7R7L, .yCE8d, ._3dEHb, .t48Bo, .PdTAI, .mAUDl, ._2z6nI, .abaSk, .y2E5d, ._8A5w5, .sH_mn, .j_2Hd, .r5z8m, ._6Pc63, .M08iG, .zVbeI, .JfBqt, .fuQUr:hover, ._914pk, .eiUFA, .zGtbP {border-color: #333;}

._4sf0r, ._nx5in, ._ouv75, ._psd08, ._s5vjd, ._dnf8p, .Di7vw, .XTCLo, .vtWDf, .mOBkM, ._41KYi, .pV7Qt {border-color: #1e1e1e;}

._9GP1n, .gr27e, ._lz6s, .YQf7h, .IPQK5 {border-color: #1a1a1a;}

.AcVnq, .KQA-S, ._1MzIy, ._3MPWk, ._8Rm4L, .zMhqu, .UE9AK, .hUQsm, .ltEKP .QBXjJ, .t0fbY, .BvMHM, .drKGC, .uUIL6, :not(.Fzijm).bR_3v, .b2rUF, .JyscU.ePUX4 .eo2As,.JyscU.ePUX4 .UE9AK {border-color: #171717;}

.-HRM- {border-color: red;}

.T5hFd {border-color: transparent transparent #171717;}

 ._-1O2x {border-color: #333 !important;}

._2hvTZ, ._9GP1n, .izU2O, .nJAzx, .nJAzx:visited, .gmFkV, .gmFkV:active, .gmFkV:focus, .gmFkV:hover, .gmFkV:visited, .jQgLo, .ho19H, .ho19H:active, .ho19H:focus, .ho19H:hover, .ho19H:visited, .Xl2Pu, .Qj3-a, .Qj3-a:visited, .zwlfE, .g47SY, .-fzfL, a.T-jvg, a.T-jvg:visited, .dsJ8D, .piCib, .XTCLo, .cqXBL, .cqXBL:visited, .y9v3U, a.F2iT8, .id8oV, .YFq-A, .yrJyr, .yrJyr:visited, .lEGIs, ._5jcYX, .HSDi9, .ptjOt, .olKGW, .DXv8P, .DXv8P:visited, .zsYNt, .zsYNt:visited, ._32eiM, ._32eiM:visited, .VIsJD, .rkEop, .h-aRd, .h-aRd:active, .h-aRd:hover, .h-aRd:visited, .sxIVS, .tiXqb, .JLJ-B, .p7vTm, .kHYQv, .zOJg-, .ada5V, .XX1Wc, .psMFr, .SbZN3, .I6l20, .nsKSz, .X381g .hbztf, .ufStW, .qlmO5, .XAiP-, .uyeeR, .Ypffh, .qPANj, .RPhNB, .t48Bo, a.O4GlU, a.O4GlU:visited, .YaGYu, .T030x, .XdXBI, .rhpdm, .K3Sf1, .-vDIg, ._34G9B, ._34G9B:visited, ._06yVv, .neTWR, ._0G-TY, ._9Ytll, ._8A5w5, .j_2Hd, .tT89j, .wU9Zd, ._sneK, ._9mS2q, .MztBt, .M08iG, .CcMMZ, .eXle2, .C4VMK, ._914pk, ._0imsa, ._0imsa:visited, .yVvXQ, .KV-D4, .SZRPf, .yWX7d._8A5w5, a.yWX7d._8A5w5, a.yWX7d._8A5w5:visited, .hI7cq, .hI7cq:visited, .cqXBL, .cqXBL:visited, .y9v3U, .RucPH, .RucPH:visited, .pKCwU, .Nm9Fw, .m82CD, .KV-D4, .BcJ68, .jkw7z {color: white;}

.TlrDj, .TlrDj:visited, .zV_Nj, .zV_Nj:visited, .QxuJw {color: #c9c9c9;}

.aD2cN::after, ._0T_XJ::before, ._0T_XJ::after {background-image: none;}

.hUQsm {box-shadow: none;}

._9AhH0, ._lz6s, .eLAPa {box-shadow: 0px 0px 30px 1px black;}

._6e4x5, ._9dpug, ._hql7s, ._o2wxh, ._75ljm::after, ._ml0fm, ._lfwfo, ._4kplh ._9dpug {border-bottom: 1px solid #333;}

._km7ip, ._4kplh ._8oo9w, ._km7ip, ._qwuqp {border-top: 1px solid #333;}

._jh9m1:focus, ._etlo6:hover, ._4abhr:focus, ._jlcqs:focus, ._nlo2g:focus, ._3jk0j, ._pg23k::after, ._qbbek {border-color: #292929;}

._nx5in, ._ouv75, ._psd08, ._s5vjd, ._lyjak, ._p4iax {border-color: #181818;}

._69lq6._c1jwo, .T-jvg, .d-Vzv {border-color: white;}

._jcvs2 {border-radius: 0px;}

._hrzod {display : none;}

._9apn1 {border-color: transparent transparent #1b1b1b;}

._hr9tt, ._780fm, ._pg23k, .RO68f {background: #1e1e1e;}

._sq5zx, ._4pxed {background-color: #313030;}

._d1a9t {height: 45px;}

._9c1ca {border-color: #3897F0;}

a, a:visited {color: #005fbd;}

";
                    break;

                case "WhatsApp":
                    cssToApply += @"
.intro-image, .tail.message-in .tail-container, .message-out .tail-container, .intro-secondary, .butterbar, ._1Y6o1, ._2Uo0Z, html[dir] ._1wSzK, .tail-container, ._3UQPd {display: none;}

.app-wrapper-web .app {width: auto; top: 0px; height: 100%;}

html[dir] ._3Jvyf, html[dir] ._3RiLE, html[dir] .ZP8RM, html[dir] ._2UaNq, html[dir] ._3fs0K, html[dir] ._13mgZ, html[dir] ._3Fq9Y.iEUx2, html[dir] .kiodY, html[dir] ._1KDYa, html[dir] .ZP8RM._19OGD, html[dir] .cGLoy, html[dir] ._2UaNq._1k-OW:hover, html[dir] ._19xqi {background: #1a1a1a;}

html[dir] .iFKgT, html[dir] ._2UaNq._3mMX1, html[dir] ._1_keJ, html[dir] ._1Kstu, html[dir] ._2AJf5 {background: #171717;}

html[dir] ._2hHc6 {background: #181818;}

html[dir] ._2i7Ej,  html[dir] .eiCXe, html[dir] ._2zCfw, html[dir] ._1ebw2, html[dir] .OWYLS, html[dir] ._2LSbZ,  html[dir] ._26JG5, html[dir] .r7sRK, html[dir] ._3ZJOk, html[dir] ._11GZy, html[dir] .GCxhK, html[dir] .message-in ._2Wx_5, html[dir] .message-out ._2Wx_5, html[dir] .message-in ._2Hp95 ._3Mf7Z, html[dir] .message-in ._2Hp95 ._3qAvH {background : #1e1e1e;}

html[dir] .message-out, html[dir] .message-in {background: #171717;}

html[dir] ._2UaNq._16_lP, html[dir] ._2UaNq:hover, html[dir] .a7otO, html[dir] ._3BqnP._3VXiW, html[dir] ._26JG5:hover, html[dir] .ZP8RM::after, html[dir] ._3ZVgT, html[dir] ._1WMT2._1lakC, html[dir] ._1WMT2._2nA3s {background: #292929;}

html[dir] ._2WP9Q, html[dir] ._2UaNq._16_lP ._2WP9Q, html[dir] ._2UaNq:hover ._2WP9Q, html[dir] ._2UaNq._3mMX1 ._2WP9Q, html[dir] ._2UaNq._3mMX1::after, html[dir] ._2UaNq._3mMX1::after, html[dir] ._2UaNq._16_lP::after, html[dir] ._2UaNq:hover::after, html[dir=ltr] ._3fs0K, html[dir=ltr] .iFKgT {border-color: #292929;}

html[dir] ._3_-Si + ._3_-Si ._2x2XP, html[dir] ._26JG5 + ._26JG5 ._27Ie2, html[dir] .ZP8RM::after, html[dir] ._2DxRd {border-color: #333;}

html[dir] ._13mgZ {border-color: #1a1a1a;}

._3H4MS, ._0LqQ, ._19vo_, .i1XSV._3Q3ui, html[dir] ._13mgZ, ._1zGQT, ._3MYI2, html[dir] ._2zCfw, .Sl-9e, ._2HHbr, ._2Vo52, .kiodY .kyJvR, ._6xQdq, ._1drsQ, ._8fE-g, html[dir] ._3FeAD._3jJ43 ._3u328, html[dir=ltr] .liawM ._1be_K, html[dir=ltr] .liawM ._5KVVe, html[dir] ._3FeAD._2YgjU ._3u328, ._2ofvs, ._2ko65 ._3H4MS, ._2ko65 ._0LqQ, ._1cDWi, ._2UaNq.ROigK ._3H4MS, .xD91K, ._12pGw strong, ._2V2qB, ._1o0MN {color: white;}

.xD91K, ._2UaNq._3mMX1 .xD91K, ._2UaNq._2ko65 .xD91K, ._3mkas, ._3-cgK {color: #c2c2c2;}

html[dir] .a7otO {text-shadow: none;}

html[dir=ltr] ._15CAo._3EQsG, html[dir=ltr] ._15CAo._2Nkc4 {background: none;}

._3fs0K::after {height: none;}

html[dir] .KgevS ._0LqQ {padding-right: 20px;}

._3FB_S {color: black;}
html[dir=ltr] ._1F9Ap {color: white !important;}

._1XCAr ._3I7nT, html[dir] ._1RYPC, html[dir] ._1RYPC > div, .hjJpn, html[dir] .qfKkX, html[dir=ltr] ._30prC, html[dir=ltr] ._1mGV6, html[dir=ltr] ._3cO_w {filter: invert(1);}



";
                    break;

            }
            cssToApply = cssToApply.Replace("\r", string.Empty).Replace("\n", string.Empty);
            return cssToApply;
        }

    }
}
