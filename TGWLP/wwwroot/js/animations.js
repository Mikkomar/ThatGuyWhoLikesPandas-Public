var showCookieConsentDialog = (document.cookie.indexOf('TGWLP.ConsentCookie=') < 0);
var cookieConsentDialog;
$.noConflict();

//if (!sessionStorage.getItem('welcomeAnimationPlayed')) {
if (document.cookie.indexOf('TGWLP.WelcomeAnimationPlayed=') < 0) {
    if ($('.welcome-wrapper').length) {
        $('.welcome-wrapper').show();
        $('.page-wrapper').hide();

        var welcomeDelay = 1;
        var welcomeFadeOutDelay = 3;
        var velcomeFadeInDuration = 2;
        var velcomeFadeOutDuration = 1;
        gsap.from(
            '.welcome-wrapper',
            {
                delay: welcomeDelay,
                duration: velcomeFadeInDuration,
                opacity: 0
            }
        );

        gsap.to(
            '.welcome-wrapper',
            {
                delay: welcomeFadeOutDelay,
                duration: velcomeFadeOutDuration,
                opacity: 0
            }
        )
            .then(function () {
                $('.welcome-wrapper').hide();
                $('.page-wrapper').show();
                showCookieConsentDialogFunc();
            });
    }

    var pageDelay = welcomeFadeOutDelay + velcomeFadeOutDuration;
    var navbarDelay = welcomeFadeOutDelay + velcomeFadeOutDuration;
    var pageDuration = 1;
    var navbarDuration = 1;
    if ($('.welcome-wrapper').length) {
        gsap.from(
            '.page-wrapper',
            {
                delay: pageDelay,
                duration: pageDuration,
                y: '-200%',
            }
        );
    }

    if ($('.navbar').length) {
        gsap.from(
            '.navbar',
            {
                delay: pageDelay,
                duration: pageDuration,
                x: '-200%',
            }
        );
    }

    var pageContentDelay = welcomeFadeOutDelay + velcomeFadeOutDuration + pageDuration;
    var pageTitleDelay = pageContentDelay;
    if ($('.slide-in-left').length) {
        gsap.from(
            '.slide-in-left',
            {
                delay: pageTitleDelay,
                duration: 2,
                x: '-500%',
                ease: 'power4'
            });
    }

    if ($('.slide-in-right').length) {
        gsap.from(
            '.slide-in-right',
            {
                delay: pageTitleDelay,
                duration: 2,
                x: '500%',
                ease: 'power4'
            });
    }

    var storyPreviewDelay = pageContentDelay;
    var storyPreviewButtonDelay = pageContentDelay + 1.5;
    var storyTitleAnimationDuration = 1;
    var buttonAnimationDuration = 1;
    var paragrahpAnimationDuration = 0.5;
    var storyPreviewFrequency = 0.5;
    var paragraphFrequency = 0.5;
    if ($('.story-preview-wrapper').length) {
        $(document).find('.story-preview-wrapper').each(function (previewIndex) {
            var storyPreviewParagraphDelay = 1;
            gsap.from(
                $(this).find('.story-preview-title'),
                {
                    delay: storyPreviewDelay,
                    duration: storyTitleAnimationDuration,
                    x: '-100%',
                    ease: 'power4'
                }
            );

            gsap.from(
                $(this).find('.story-preview-button'),
                {
                    delay: storyPreviewButtonDelay,
                    duration: buttonAnimationDuration,
                    y: '500%',
                    ease: 'power4'
                }
            );

            $(this).find('.storyPreviewText').each(function () {
                $(this).find('p').each(function (paragraphIndex) {
                    gsap.from(
                        this,
                        {
                            delay: storyPreviewDelay + storyPreviewParagraphDelay,
                            duration: paragrahpAnimationDuration,
                            y: '500%',
                            ease: 'power4'
                        }
                    );
                    storyPreviewParagraphDelay += paragraphFrequency;
                });
            });
            storyPreviewDelay += storyPreviewFrequency;
        });
    }

    var storyPanelDelay = storyPreviewDelay;
    var storyPanelFrequency = 0.2;
    var storyPanelDuration = 1;
    if ($('.story-panel').length) {
        $('.story-panel').find('.story-panel-wrapper').each(function () {
            gsap.from(
                this,
                {
                    delay: storyPanelDelay,
                    duration: storyPanelDuration,
                    opacity: 0,
                    x: '500%'
                }
            );
            storyPanelDelay += storyPanelFrequency;
        });
    }

    //sessionStorage.setItem('welcomeAnimationPlayed', true);
    setCookie('TGWLP.WelcomeAnimationPlayed', 'true', 30);
}
else {
    showCookieConsentDialogFunc();
}

var storyTitleDelay = 0;
var storyDateDelay = 0;
var storyTitleDuration = 0.5;
if ($('.story-title').length) {
    gsap.from(
        $('.story-title').find('h1'),
        {
            delay: storyTitleDelay,
            duration: storyTitleDuration,
            y: '-500px',
            ease: 'power4'
        }
    );
}

if ($('.story-title').length) {
    gsap.from(
        $('.story-title').find('p'),
        {
            delay: storyDateDelay,
            duration: storyTitleDuration,
            y: '-500%',
            ease: 'power4'
        }
    );
}

var storyParagraphDelay = 0.25;
var storyParagraphDuration = 1;
var storyParagraphFrequenzy = 0.1;
if ($('.story-wrapper').length) {
    $('.story-wrapper').find('.story-text').find('p').each(function () {
        gsap.from(
            this,
            {
                delay: storyParagraphDelay,
                duration: storyParagraphDuration,
                x: '-500%',
                ease: Back.easeOut.config(0.8)
            }
        );
        storyParagraphDelay += storyParagraphFrequenzy;
    });
}

var bookWrapperDelay = storyParagraphDelay;
var bookWrapperDurateion = 1;
var bookCardDelay = 0;
if ($('.book-wrapper').length) {
    gsap.from(
        $('.book-wrapper'),
        {
            delay: bookWrapperDelay,
            duration: bookWrapperDurateion,
            y: '500%'
        }
    );
}

function showCookieConsentDialogFunc() {
    if (showCookieConsentDialog) {
        cookieConsentDialog = $('.cookieConsent-wrapper').dialog({
            height: 'auto',
            width: '70vw',
            dialogClass: 'cookieConsent-wrapper'
        });

        var button = $("#btnCookieConsentAccept");
        button.on('click', function () {
            document.cookie = button.data('cookieString');
            cookieConsentDialog.dialog('close');
        });
    }
}