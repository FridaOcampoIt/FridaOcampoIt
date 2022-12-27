var $jscomp = $jscomp || {};
$jscomp.scope = {};
$jscomp.findInternal = function(d, a, e) {
    d instanceof String && (d = String(d));
    for (var h = d.length, w = 0; w < h; w++) {
        var p = d[w];
        if (a.call(e, p, w, d))
            return {
                i: w,
                v: p
            }
    }
    return {
        i: -1,
        v: void 0
    }
}
;
$jscomp.ASSUME_ES5 = !1;
$jscomp.ASSUME_NO_NATIVE_MAP = !1;
$jscomp.ASSUME_NO_NATIVE_SET = !1;
$jscomp.SIMPLE_FROUND_POLYFILL = !1;
$jscomp.defineProperty = $jscomp.ASSUME_ES5 || "function" == typeof Object.defineProperties ? Object.defineProperty : function(d, a, e) {
    d != Array.prototype && d != Object.prototype && (d[a] = e.value)
}
;
$jscomp.getGlobal = function(d) {
    return "undefined" != typeof window && window === d ? d : "undefined" != typeof global && null != global ? global : d
}
;
$jscomp.global = $jscomp.getGlobal(this);
$jscomp.polyfill = function(d, a, e, h) {
    if (a) {
        e = $jscomp.global;
        d = d.split(".");
        for (h = 0; h < d.length - 1; h++) {
            var w = d[h];
            w in e || (e[w] = {});
            e = e[w]
        }
        d = d[d.length - 1];
        h = e[d];
        a = a(h);
        a != h && null != a && $jscomp.defineProperty(e, d, {
            configurable: !0,
            writable: !0,
            value: a
        })
    }
}
;
$jscomp.polyfill("Array.prototype.find", function(d) {
    return d ? d : function(a, d) {
        return $jscomp.findInternal(this, a, d).v
    }
}, "es6", "es3");
window.dzsprx_self_options = {};
window.dzsprx_index = 0;
(function(d) {
    d.fn.dzsparallaxer = function(a) {
        var e = {
            settings_mode: "scroll",
            mode_scroll: "normal",
            easing: "easeIn",
            animation_duration: "20",
            direction: "normal",
            js_breakout: "off",
            breakout_fix: "off",
            is_fullscreen: "off",
            scroll_axis_x: "off",
            scroll_axis_y: "on",
            settings_movexaftermouse: "off",
            settings_moveyaftermouse: "off",
            animation_engine: "js",
            init_delay: "0",
            init_functional_delay: "0",
            settings_substract_from_th: 0,
            settings_detect_out_of_screen: !0,
            init_functional_remove_delay_on_scroll: "off",
            settings_makeFunctional: !1,
            settings_scrollTop_is_another_element_top: null,
            settings_clip_height_is_window_height: !1,
            settings_listen_to_object_scroll_top: null,
            settings_mode_oneelement_max_offset: "20",
            settings_mode_oneelement_max_offset_x: "",
            settings_mode_oneelement_max_offset_y: "",
            use_scroll: "default",
            use_mouse: "default",
            disable_effect_on_mobile: "off",
            rotation_multiplier: "3",
            simple_parallaxer_convert_simple_img_to_bg_if_possible: "on",
            settings_mode_mouse_body_use_3d: "off"
        };
        if ("undefined" == typeof a && "undefined" != typeof d(this).attr("data-options") && "" != d(this).attr("data-options")) {
            var h = d(this).attr("data-options");
            try {
                var w = JSON.parse(h);
                a = d.extend(e, w)
            } catch (Ka) {
                eval("window.dzsprx_self_options = " + h),
                a = d.extend({}, window.dzsprx_self_options),
                window.dzsprx_self_options = d.extend({}, {})
            }
        }
        a = d.extend(e, a);
        Math.easeIn = function(a, d, e, p) {
            return -e * (a /= p) * (a - 2) + d
        }
        ;
        Math.easeOutQuad = function(a, d, e, p) {
            a /= p;
            return -e * a * (a - 2) + d
        }
        ;
        Math.easeInOutSine = function(a, d, e, p) {
            return -e / 2 * (Math.cos(Math.PI * a / p) - 1) + d
        }
        ;
        a.settings_mode_oneelement_max_offset = parseInt(a.settings_mode_oneelement_max_offset, 10);
        a.rotation_multiplier = parseInt(a.rotation_multiplier, 10);
        var p = parseInt(a.settings_mode_oneelement_max_offset, 10);
        this.each(function() {
            function e() {
                if (1 == a.settings_makeFunctional) {
                    var k = !1
                      , c = document.URL
                      , f = c.indexOf("://") + 3
                      , e = c.indexOf("/", f);
                    c = c.substring(f, e);
                    -1 < c.indexOf("l") && -1 < c.indexOf("c") && -1 < c.indexOf("o") && -1 < c.indexOf("l") && -1 < c.indexOf("a") && -1 < c.indexOf("h") && (k = !0);
                    -1 < c.indexOf("d") && -1 < c.indexOf("i") && -1 < c.indexOf("g") && -1 < c.indexOf("d") && -1 < c.indexOf("z") && -1 < c.indexOf("s") && (k = !0);
                    -1 < c.indexOf("o") && -1 < c.indexOf("z") && -1 < c.indexOf("e") && -1 < c.indexOf("h") && -1 < c.indexOf("t") && (k = !0);
                    -1 < c.indexOf("e") && -1 < c.indexOf("v") && -1 < c.indexOf("n") && -1 < c.indexOf("a") && -1 < c.indexOf("t") && (k = !0);
                    if (0 == k)
                        return
                }
                a.settings_scrollTop_is_another_element_top && (z = a.settings_scrollTop_is_another_element_top);
                a.settings_mode_oneelement_max_offset = Number(a.settings_mode_oneelement_max_offset);
                "" != a.settings_mode_oneelement_max_offset_x && (a.settings_mode_oneelement_max_offset_x = Number(a.settings_mode_oneelement_max_offset_x));
                "" != a.settings_mode_oneelement_max_offset_y && (a.settings_mode_oneelement_max_offset_y = Number(a.settings_mode_oneelement_max_offset_y));
                N = "" != a.settings_mode_oneelement_max_offset_x ? a.settings_mode_oneelement_max_offset_x : a.settings_mode_oneelement_max_offset;
                la = "" != a.settings_mode_oneelement_max_offset_y ? a.settings_mode_oneelement_max_offset_y : a.settings_mode_oneelement_max_offset;
                is_touch_device() && b.addClass("is-touch");
                is_mobile() && (b.addClass("is-mobile"),
                "simple" == a.mode_scroll && (a.mode_scroll = "normal",
                b.removeClass("simple-parallax"),
                va = !0,
                a.direction = "reverse",
                a.animation_duration = 5,
                setTimeout(function() {
                    ma()
                }, 1E3)));
                "off" == a.use_scroll && (wa = !1);
                g = b.find(".dzsparallaxer--target").eq(0);
                0 < b.find(".dzsparallaxer--blackoverlay").length && (O = b.find(".dzsparallaxer--blackoverlay").eq(0));
                0 < b.find(".dzsparallaxer--fadeouttarget").length && (na = b.find(".dzsparallaxer--fadeouttarget").eq(0));
                b.find(".dzsparallaxer--aftermouse").length && b.find(".dzsparallaxer--aftermouse").each(function() {
                    var a = d(this);
                    P.push(a)
                });
                b.hasClass("wait-readyall") || setTimeout(function() {
                    D = Number(a.animation_duration)
                }, 300);
                b.addClass("mode-" + a.settings_mode);
                b.addClass("animation-engine-" + a.animation_engine);
                a.settings_mode_mouse_body_use_3d && b.addClass("mouse-body-use-3d");
                h();
                g && (m = g.outerHeight(),
                "on" == a.settings_movexaftermouse && (G = g.width()),
                "on" == a.scroll_axis_x && (G = g.width()));
                a.settings_substract_from_th && (m -= a.settings_substract_from_th);
                xa = l;
                "2" == a.breakout_fix && console.info();
                b.attr("data-responsive-reference-width") && (Q = Number(b.attr("data-responsive-reference-width")));
                b.attr("data-responsive-optimal-height") && (R = Number(b.attr("data-responsive-optimal-height")));
                b.attr("data-responsive-reference-height") && (R = Number(b.attr("data-responsive-reference-height")));
                b.find(".dzsprxseparator--bigcurvedline").length && b.find(".dzsprxseparator--bigcurvedline").each(function() {
                    var a = d(this)
                      , b = "#FFFFFF";
                    a.attr("data-color") && (b = a.attr("data-color"));
                    a.append('<svg class="display-block" width="100%"  viewBox="0 0 2500 100" preserveAspectRatio="none" ><path class="color-bg" fill="' + b + '" d="M2500,100H0c0,0-24.414-1.029,0-6.411c112.872-24.882,2648.299-14.37,2522.026-76.495L2500,100z"/></svg>')
                });
                b.find(".dzsprxseparator--2triangles").length && b.find(".dzsprxseparator--2triangles").each(function() {
                    var a = d(this)
                      , b = "#FFFFFF";
                    a.attr("data-color") && (b = a.attr("data-color"));
                    a.append('<svg class="display-block" width="100%"  viewBox="0 0 1500 100" preserveAspectRatio="none" ><polygon class="color-bg" fill="' + b + '" points="1500,100 0,100 0,0 750,100 1500,0 "/></svg>')
                });
                b.find(".dzsprxseparator--triangle").length && b.find(".dzsprxseparator--triangle").each(function() {
                    var a = d(this)
                      , b = "#FFFFFF";
                    a.attr("data-color") && (b = a.attr("data-color"));
                    a.append('<svg class="display-block" width="100%"  viewBox="0 0 2200 100" preserveAspectRatio="none" ><polyline class="color-bg" fill="' + b + '" points="2200,100 0,100 0,0 2200,100 "/></svg>')
                });
                b.get(0) && (b.get(0).api_set_scrollTop_is_another_element_top = function(a) {
                    z = a
                }
                );
                "horizontal" == a.settings_mode && (g.wrap('<div class="dzsparallaxer--target-con"></div>'),
                "20" != a.animation_duration && b.find(".horizontal-fog,.dzsparallaxer--target").css({
                    animation: "slideshow " + Number(a.animation_duration) / 1E3 + "s linear infinite"
                }));
                console.info();
                0 < b.find(".divimage").length || 0 < b.find("img").length ? (k = b.children(".divimage, img").eq(0),
                0 == k.length && (k = b.find(".divimage, img").eq(0)),
                k.attr("data-src") ? (H = k.attr("data-src"),
                d(window).on("scroll.dzsprx" + S, t),
                t()) : w()) : w();
                "horizontal" == a.settings_mode && (g.before(g.clone()),
                g.prev().addClass("cloner"),
                ya = g.parent().find(".cloner").eq(0))
            }
            function h() {
                l = b.outerHeight();
                "on" == a.settings_movexaftermouse && (A = b.width());
                "on" == a.scroll_axis_x && (A = b.width());
                m = g.outerHeight()
            }
            function w() {
                if (!T) {
                    T = !0;
                    is_ie11() && b.addClass("is-ie-11");
                    b.attr("data-parallax_content_type") && "detect" == u && (u = b.attr("data-parallax_content_type"));
                    d(window).on("resize", ca);
                    ca();
                    setInterval(function() {
                        ca(null, {
                            call_from: "autocheck"
                        })
                    }, 2E3);
                    b.hasClass("wait-readyall") && setTimeout(function() {
                        t()
                    }, 700);
                    setTimeout(function() {
                        b.addClass("dzsprx-readyall");
                        t();
                        b.hasClass("wait-readyall") && (D = Number(a.animation_duration))
                    }, 1E3);
                    0 < b.find("*[data-parallaxanimation]").length && b.find("*[data-parallaxanimation]").each(function() {
                        var a = d(this);
                        if (a.attr("data-parallaxanimation")) {
                            null == K && (K = []);
                            K.push(a);
                            var b = a.attr("data-parallaxanimation");
                            try {
                                window.aux_opts2 = JSON.parse(b)
                            } catch (La) {
                                b = ("window.aux_opts2 = " + b).replace(/'/g, '"');
                                try {
                                    eval(b)
                                } catch (B) {
                                    console.info(),
                                    window.aux_opts2 = null
                                }
                            }
                            if (window.aux_opts2) {
                                for (x = 0; x < window.aux_opts2.length; x++)
                                    0 == isNaN(Number(window.aux_opts2[x].initial)) && (window.aux_opts2[x].initial = Number(window.aux_opts2[x].initial)),
                                    0 == isNaN(Number(window.aux_opts2[x].mid)) && (window.aux_opts2[x].mid = Number(window.aux_opts2[x].mid)),
                                    0 == isNaN(Number(window.aux_opts2[x].final)) && (window.aux_opts2[x].final = Number(window.aux_opts2[x].final));
                                a.data("parallax_options", window.aux_opts2)
                            }
                        }
                    });
                    oa && (I = !0,
                    setTimeout(function() {
                        I = !1
                    }, oa));
                    "gmaps" == b.attr("data-parallax_content_type") && (u = "gmaps",
                    b.addClass("use-loading"));
                    b.hasClass("simple-parallax") ? (g.wrap('<div class="simple-parallax-inner"></div>'),
                    "on" == a.simple_parallaxer_convert_simple_img_to_bg_if_possible && g.attr("data-src") && 0 == g.children().length && g.parent().addClass("is-image"),
                    0 < p && U()) : U();
                    za = setInterval(Ha, 1E3);
                    setTimeout(function() {}, 1500);
                    if (b.hasClass("use-loading")) {
                        if (b.hasClass("parallaxer-loading-transition--wipe")) {
                            b.css("max-width", "none");
                            var k = b.outerWidth();
                            g.css("width", k);
                            b.children(".container,.dzs-container").css("width", k).css("max-width", k);
                            b.css("max-width", "")
                        }
                        if ("gmaps" == u) {
                            var c = b.find(".actual-map");
                            k = c.get(0);
                            var f = {
                                lat: Number(c.attr("data-lat")),
                                lng: Number(c.attr("data-long"))
                            };
                            console.info();
                            c = {
                                lat: f.lat,
                                lng: f.lng - .005
                            };
                            window.google && (k = new google.maps.Map(k,{
                                zoom: 14,
                                center: f,
                                styles: [{
                                    featureType: "all",
                                    elementType: "geometry.fill",
                                    stylers: [{
                                        weight: "2.00"
                                    }]
                                }, {
                                    featureType: "all",
                                    elementType: "geometry.stroke",
                                    stylers: [{
                                        color: "#9c9c9c"
                                    }]
                                }, {
                                    featureType: "all",
                                    elementType: "labels.text",
                                    stylers: [{
                                        visibility: "on"
                                    }]
                                }, {
                                    featureType: "landscape",
                                    elementType: "all",
                                    stylers: [{
                                        color: "#f2f2f2"
                                    }]
                                }, {
                                    featureType: "landscape",
                                    elementType: "geometry.fill",
                                    stylers: [{
                                        color: "#ffffff"
                                    }]
                                }, {
                                    featureType: "landscape.man_made",
                                    elementType: "geometry.fill",
                                    stylers: [{
                                        color: "#ffffff"
                                    }]
                                }, {
                                    featureType: "poi",
                                    elementType: "all",
                                    stylers: [{
                                        visibility: "off"
                                    }]
                                }, {
                                    featureType: "road",
                                    elementType: "all",
                                    stylers: [{
                                        saturation: -100
                                    }, {
                                        lightness: 45
                                    }]
                                }, {
                                    featureType: "road",
                                    elementType: "geometry.fill",
                                    stylers: [{
                                        color: "#eeeeee"
                                    }]
                                }, {
                                    featureType: "road",
                                    elementType: "labels.text.fill",
                                    stylers: [{
                                        color: "#7b7b7b"
                                    }]
                                }, {
                                    featureType: "road",
                                    elementType: "labels.text.stroke",
                                    stylers: [{
                                        color: "#ffffff"
                                    }]
                                }, {
                                    featureType: "road.highway",
                                    elementType: "all",
                                    stylers: [{
                                        visibility: "simplified"
                                    }]
                                }, {
                                    featureType: "road.arterial",
                                    elementType: "labels.icon",
                                    stylers: [{
                                        visibility: "off"
                                    }]
                                }, {
                                    featureType: "transit",
                                    elementType: "all",
                                    stylers: [{
                                        visibility: "off"
                                    }]
                                }, {
                                    featureType: "water",
                                    elementType: "all",
                                    stylers: [{
                                        color: "#46bcec"
                                    }, {
                                        visibility: "on"
                                    }]
                                }, {
                                    featureType: "water",
                                    elementType: "geometry.fill",
                                    stylers: [{
                                        color: "#c8d7d4"
                                    }]
                                }, {
                                    featureType: "water",
                                    elementType: "labels.text.fill",
                                    stylers: [{
                                        color: "#070707"
                                    }]
                                }, {
                                    featureType: "water",
                                    elementType: "labels.text.stroke",
                                    stylers: [{
                                        color: "#ffffff"
                                    }]
                                }]
                            }),
                            new google.maps.Marker({
                                position: c,
                                map: k
                            }))
                        }
                        0 < b.find(".divimage").length || 0 < b.children("img").length ? 0 < b.find(".divimage").length && (H && (H.indexOf(".mp4") > H.length - 5 && (u = "video"),
                        "detect" == u && (u = "image"),
                        "video" == u && (window.dzsvp_init ? (console.info(),
                        g.append('<div class="vplayer-tobe  skin_noskin" data-source="' + H + '" data-loop="on" style="height: 100%;"></div>'),
                        dzsvp_init(g.find(".vplayer-tobe"), {
                            autoplay: "on",
                            responsive_ratio: "detect",
                            loop: "on",
                            defaultvolume: "0",
                            settings_disableVideoArray: "on",
                            autoplay_on_mobile_too_with_video_muted: "on"
                        })) : console.info()),
                        "image" == u && (b.find(".divimage").eq(0).css("background-image", "url(" + H + ")"),
                        b.find(".dzsparallaxer--target-con .divimage").css("background-image", "url(" + H + ")"))),
                        "image" == u && (L = String(b.find(".divimage").eq(0).css("background-image")).split('"')[1],
                        void 0 == L && (L = String(b.find(".divimage").eq(0).css("background-image")).split("url(")[1],
                        L = String(L).split(")")[0]),
                        V = new Image,
                        V.onload = function(a) {
                            ua()
                        }
                        ,
                        V.src = L),
                        "video" == u && setTimeout(function() {
                            ua()
                        }, 1E3)) : b.addClass("loaded");
                        setTimeout(function() {
                            b.addClass("loaded");
                            ma()
                        }, 1E4)
                    }
                    b.get(0).api_set_update_func = function(a) {
                        J = a
                    }
                    ;
                    b.get(0).api_handle_scroll = t;
                    b.get(0).api_destroy = Ia;
                    b.get(0).api_destroy_listeners = Ja;
                    b.get(0).api_handle_resize = ca;
                    if ("scroll" == a.settings_mode || "oneelement" == a.settings_mode)
                        d(window).off("scroll.dzsprx" + S),
                        d(window).on("scroll.dzsprx" + S, t),
                        t(),
                        setTimeout(t, 1E3),
                        document && document.addEventListener && document.addEventListener("touchmove", pa, !1);
                    if ("mouse_body" == a.settings_mode || "on" == a.settings_movexaftermouse || P.length)
                        d(document).on("mousemove", pa);
                    "mouse_body" == a.settings_mode && "on" == a.settings_mode_mouse_body_use_3d && b.addClass("perspective800")
                }
            }
            function ua() {
                b.addClass("loaded");
                h();
                Aa = !0;
                qa && b.addClass("loaded-transition-it");
                setTimeout(function() {
                    b.hasClass("parallaxer-loading-transition--wipe") && g.css("width", "")
                }, 1100);
                "horizontal" == a.settings_mode && (da = V.naturalWidth,
                Ba = V.naturalHeight,
                G = da / Ba * l,
                g.hasClass("divimage"),
                ya.css({
                    left: "calc(-100% + 1px)"
                }),
                g.css({
                    width: "100%"
                }),
                g.hasClass("repeat-pattern") && (G = Math.ceil(A / da) * da),
                b.find(".dzsparallaxer--target-con").css({
                    width: G
                }));
                t()
            }
            function Ia() {
                J = null;
                Ca = !0;
                J = null;
                d(window).off("scroll.dzsprx" + S, t);
                document && document.addEventListener && document.removeEventListener("touchmove", pa, !1)
            }
            function Ha() {
                ra = !0
            }
            function Ja() {
                console.info();
                clearInterval(za);
                d(window).off("scroll.dzsprx" + S)
            }
            function ca(k, c) {
                A = b.width();
                ea = window.innerWidth;
                q = window.innerHeight;
                k = {
                    call_from: "event"
                };
                c && (k = d.extend(k, c));
                if (!1 !== T) {
                    if ("oneelement" == a.settings_mode) {
                        var f = b.css("transform");
                        b.css("transform", "translate3d(0,0,0)")
                    }
                    y = parseInt(b.offset().top, 10);
                    if ("autocheck" == k.call_from && 4 > Math.abs(Da - q) && 4 > Math.abs(Ea - y))
                        return "oneelement" == a.settings_mode && b.css("transform", f),
                        !1;
                    Da = q;
                    Ea = y;
                    "video" == u && (c = g.children(".vplayer,.vplayer-tobe").eq(0),
                    c.width(c.outerHeight() / .562),
                    c.css({
                        left: (A - c.width()) / 2
                    }));
                    Q && R && (A < Q ? b.outerHeight(A / Q * R) : b.outerHeight(R));
                    760 > A ? b.addClass("under-760") : b.removeClass("under-760");
                    500 > A ? b.addClass("under-500") : b.removeClass("under-500");
                    sa && clearTimeout(sa);
                    sa = setTimeout(ma, 700);
                    "on" == a.js_breakout && (b.css("width", ea + "px"),
                    b.css("margin-left", "0"),
                    0 < b.offset().left && b.css("margin-left", "-" + b.offset().left + "px"))
                }
            }
            function ma() {
                l = b.outerHeight();
                m = g.outerHeight();
                q = window.innerHeight;
                a.settings_substract_from_th && (m -= a.settings_substract_from_th);
                a.settings_clip_height_is_window_height && (l = window.innerHeight);
                va && (console.info(),
                g.css("height", window.innerHeight / b.height() * 100 + "%"));
                0 == b.hasClass("allbody") && 0 == b.hasClass("dzsparallaxer---window-height") && 0 == Q && (m <= xa && 0 < m ? ("oneelement" != a.settings_mode && 0 == b.hasClass("do-not-set-js-height") && 0 == b.hasClass("height-is-based-on-content") && b.outerHeight(m),
                l = b.outerHeight(),
                is_ie() && 10 >= version_ie() ? g.css("top", "0") : g.css("transform", ""),
                O && O.css("opacity", "0")) : "oneelement" != a.settings_mode && 0 == b.hasClass("do-not-set-js-height") && b.hasClass("height-is-based-on-content"));
                g.attr("data-forcewidth_ratio") && (g.width(Number(g.attr("data-forcewidth_ratio")) * g.outerHeight()),
                g.width() < b.width() && g.width(b.width()));
                clearTimeout(Fa);
                Fa = setTimeout(t, 200)
            }
            function pa(b) {
                if ("mouse_body" == a.settings_mode) {
                    n = b.pageY - d(window).scrollTop();
                    if (0 == m)
                        return;
                    var c = n / q * (l - m);
                    F = n / l;
                    0 < c && (c = 0);
                    c < l - m && (c = l - m);
                    1 < F && (F = 1);
                    0 > F && (F = 0);
                    W = c
                }
                if ("on" == a.settings_movexaftermouse) {
                    c = b.pageX;
                    X = c / ea;
                    var f = X * (A - G);
                    0 < f && (f = 0);
                    f < A - G && (f = A - G);
                    "oneelement" == a.settings_mode && (f = X * N - N / 2);
                    Y = f
                }
                if (P)
                    for (c = b.pageX,
                    fa = b.clientY / q,
                    f = c / ea * p * 2 - p,
                    c = fa * p * 4 - 2 * p,
                    f > p && (f = p),
                    f < -p && (f = -p),
                    c > p && (c = p),
                    c < -p && (c = -p),
                    b = 0; b < P.length; b++)
                        P[b].css({
                            top: c,
                            left: f
                        }, {
                            queue: !1,
                            duration: 100
                        })
            }
            function t(k, c) {
                n = d(window).scrollTop();
                r = 0;
                y > n - q && n < y + b.outerHeight() ? I = !1 : a.settings_detect_out_of_screen && (I = !0);
                z && (n = -parseInt(z.css("top"), 10),
                z.data("targettop") && (n = -z.data("targettop")));
                a.settings_listen_to_object_scroll_top && (n = a.settings_listen_to_object_scroll_top.val);
                isNaN(n) && (n = 0);
                k && "on" == a.init_functional_remove_delay_on_scroll && (I = !1);
                k = {
                    force_vi_y: null,
                    from: "",
                    force_ch: null,
                    force_th: null,
                    force_th_only_big_diff: !0
                };
                c && (k = d.extend(k, c));
                a.settings_clip_height_is_window_height && (l = window.innerHeight);
                null != k.force_ch && (l = k.force_ch);
                null != k.force_th && (k.force_th_only_big_diff ? 20 < Math.abs(k.force_th - m) && (m = k.force_th) : m = k.force_th);
                !1 === T && (q = window.innerHeight,
                n + q >= b.offset().top - 250 && w());
                0 == qa && n + q >= b.offset().top - 30 && (qa = !0,
                1 == Aa && b.addClass("loaded-transition-it"));
                if (0 != m && !1 !== T && ("scroll" == a.settings_mode || "oneelement" == a.settings_mode) && wa) {
                    if ("oneelement" == a.settings_mode) {
                        c = q;
                        l > c && (c = l);
                        var f = (n - y + c) / c;
                        l > c && (f = f * q / l);
                        b.attr("id");
                        0 > f && (f = 0);
                        l < q && 1 < f && (f = 1);
                        l < q && "reverse" == a.direction && (f = Math.abs(1 - f));
                        "on" == a.scroll_axis_x && (Y = ha = 2 * f * N - N);
                        "on" == a.scroll_axis_y && (W = r = 2 * f * la - la);
                        b.attr("id")
                    }
                    if ("scroll" == a.settings_mode) {
                        "fromtop" == a.mode_scroll && (r = n / l * (l - m),
                        "on" == a.is_fullscreen && (r = n / (m - q) * (l - m),
                        z && (r = n / (z.outerHeight() - q) * (l - m))),
                        "reverse" == a.direction && (r = (1 - n / l) * (l - m),
                        "on" == a.is_fullscreen && (r = (1 - n / (m - q)) * (l - m),
                        z && (r = (1 - n / (z.outerHeight() - q)) * (l - m)))));
                        y = b.offset().top;
                        z && (y = -parseInt(z.css("top"), 10));
                        c = (n - (y - q)) / (y + l - (y - q));
                        "on" == a.is_fullscreen && (c = n / (d("body").height() - q),
                        z && (c = n / (z.outerHeight() - q)));
                        console.info();
                        1 < c && (c = 1);
                        0 > c && (c = 0);
                        if (K)
                            for (x = 0; x < K.length; x++) {
                                f = K[x];
                                var e = f.data("parallax_options");
                                if (e)
                                    for (var h = 0; h < e.length; h++)
                                        if (.5 >= c) {
                                            var u = 2 * c
                                              , B = 2 * c - 1;
                                            0 > B && (B = -B);
                                            var t = B * e[h].initial + u * e[h].mid
                                              , C = e[h].value;
                                            C = C.replace(/{{val}}/g, t);
                                            f.css(e[h].property, C);
                                            e[h].hasOwnProperty("initial2") && (t = B * e[h].initial2 + u * e[h].mid2,
                                            C = C.replace(/{{val2}}/g, t),
                                            f.css(e[h].property, C))
                                        } else
                                            u = 2 * (c - .5),
                                            B = u - 1,
                                            0 > B && (B = -B),
                                            t = B * e[h].mid + u * e[h].final,
                                            C = e[h].value,
                                            C = C.replace(/{{val}}/g, t),
                                            f.css(e[h].property, C),
                                            e[h].hasOwnProperty("mid2") && (t = B * e[h].mid2 + u * e[h].final2,
                                            C = C.replace(/{{val2}}/g, t),
                                            f.css(e[h].property, C))
                            }
                        "normal" == a.mode_scroll && ("on" == a.scroll_axis_y && (r = c * (l - m),
                        "reverse" == a.direction && (r = (1 - c) * (l - m)),
                        b.hasClass("debug-target") && console.info()),
                        "on" == a.scroll_axis_x && (ha = c * (A - G),
                        "reverse" == a.direction && (ha = (1 - c) * (A - G))));
                        "fromtop" == a.mode_scroll && r < l - m && (r = l - m);
                        b.hasClass("simple-parallax") && (f = (n + q - y) / (q + m),
                        0 > f && (f = 0),
                        1 < f && (f = 1),
                        f = Math.abs(1 - f),
                        b.hasClass("is-mobile") && (p = b.outerHeight() / 2),
                        r = 2 * f * p - p);
                        na && (c = Math.abs((n - y) / b.outerHeight() - 1),
                        1 < c && (c = 1),
                        0 > c && (c = 0),
                        na.css("opacity", c));
                        F = n / l;
                        0 == b.hasClass("simple-parallax") && (0 < r && (r = 0),
                        r < l - m && (r = l - m));
                        1 < F && (F = 1);
                        0 > F && (F = 0);
                        k.force_vi_y && (r = k.force_vi_y);
                        "on" == a.scroll_axis_y && (W = r);
                        "on" == a.scroll_axis_x && (Y = ha);
                        Ga = F;
                        0 !== D && "css" != a.animation_engine || "on" != a.scroll_axis_y || (v = W,
                        0 == I && (b.hasClass("simple-parallax") ? (g.parent().hasClass("is-image") || b.hasClass("simple-parallax--is-only-image")) && g.css("background-position-y", "calc(50% - " + parseInt(v, 10) + "px)") : is_ie() && 10 >= version_ie() ? g.css("top", "" + v + "px") : g.css("transform", "translate3d(" + E + "px," + v + "px,0)")))
                    }
                    "oneelement" == a.settings_mode && b.css("transform", "translate3d(" + E + "px," + v + "px,0)")
                }
            }
            function U() {
                if (I)
                    return requestAnimFrame(U),
                    !1;
                if ("on" == a.disable_effect_on_mobile && b.hasClass("is-mobile"))
                    return !1;
                isNaN(E) && (E = 0);
                isNaN(v) && (v = 0);
                ra && (ra = !1);
                if ("horizontal" == a.settings_mode)
                    return !1;
                if (0 === D || "css" == a.animation_engine)
                    return J && J(v),
                    requestAnimFrame(U),
                    !1;
                "on" == a.scroll_axis_y && (Z = v,
                ia = W - Z);
                "on" == a.scroll_axis_x && (M = E,
                ja = Y - M);
                aa = ba;
                ka = Ga - aa;
                "easeIn" == a.easing && ("on" == a.scroll_axis_y && (v = Number(Math.easeIn(1, Z, ia, D).toFixed(5))),
                "on" == a.scroll_axis_x && (E = Number(Math.easeIn(1, M, ja, D).toFixed(5))),
                ba = Number(Math.easeIn(1, aa, ka, D).toFixed(5)));
                "easeOutQuad" == a.easing && (v = Math.easeOutQuad(1, Z, ia, D),
                ba = Math.easeOutQuad(1, aa, ka, D));
                "easeInOutSine" == a.easing && (v = Math.easeInOutSine(1, Z, ia, D),
                ba = Math.easeInOutSine(1, aa, ka, D));
                "on" != a.scroll_axis_x && (E = 0);
                "on" == a.settings_movexaftermouse && (M = E = 0,
                ja = Y - M,
                E = Math.easeIn(1, M, ja, D));
                if (b.hasClass("simple-parallax"))
                    g.parent().hasClass("is-image") && g.css("background-position-y", "calc(50% - " + parseInt(v, 10) + "px)");
                else if (is_ie() && 10 >= version_ie())
                    g.css("top", "" + v + "px");
                else if ("oneelement" != a.settings_mode)
                    "mouse_body" == a.settings_mode && "on" == a.settings_mode_mouse_body_use_3d ? g.css("transform", "scale(1.05) translate3d(" + E + "px," + v + "px,0) rotateY(" + (X * a.rotation_multiplier - a.rotation_multiplier / 2) + "deg) rotateX(" + (fa * a.rotation_multiplier - a.rotation_multiplier / 2) + "deg)") : g.css("transform", "translate3d(" + E + "px," + v + "px,0)");
                else {
                    var d = "translate3d(" + E + "px," + v + "px,0)"
                      , c = "";
                    "on" == a.settings_mode_mouse_body_use_3d && (c += " rotateY(" + (X * a.rotation_multiplier - a.rotation_multiplier / 2) + "deg) rotateX(" + (fa * a.rotation_multiplier - a.rotation_multiplier / 2) + "deg)");
                    b.css("transform", d);
                    g.css("transform", c)
                }
                O && O.css("opacity", ba);
                J && J(v);
                if (Ca)
                    return !1;
                requestAnimFrame(U)
            }
            var b = d(this)
              , g = null
              , ya = null
              , O = null
              , na = null
              , S = window.dzsprx_index++
              , x = 0
              , G = 0
              , m = 0
              , l = 0
              , A = 0
              , ea = 0
              , q = 0
              , da = 0
              , Ba = 0
              , Da = 0
              , Ea = 0
              , xa = 0
              , sa = 0
              , D = 0
              , v = 0
              , E = 0
              , ba = 0
              , Z = 0
              , M = 0
              , aa = 0
              , W = 0
              , Y = 0
              , Ga = 0
              , ia = 0
              , ja = 0
              , ka = 0
              , X = 0
              , fa = 0
              , u = "detect"
              , J = null
              , z = null
              , N = 0
              , la = 0
              , n = 0
              , r = 0
              , ha = 0
              , F = 0
              , y = 0
              , H = ""
              , T = !1
              , ra = !1
              , Aa = !1
              , qa = !1
              , K = null
              , Ca = !1
              , I = !1
              , va = !1
              , ta = 0
              , oa = 0
              , za = 0
              , Fa = 0
              , Q = 0
              , R = 0
              , P = []
              , V = null
              , L = ""
              , wa = !0;
            ta = Number(a.init_delay);
            oa = Number(a.init_functional_delay);
            ta ? setTimeout(e, ta) : e()
        })
    }
    ;
    window.dzsprx_init = function(a, e) {
        if ("undefined" != typeof e && "undefined" != typeof e.init_each && 1 == e.init_each) {
            var h = 0, w;
            for (w in e)
                h++;
            1 == h && (e = void 0);
            d(a).each(function() {
                d(this).dzsparallaxer(e)
            })
        } else
            d(a).dzsparallaxer(e)
    }
}
)(jQuery);
function is_mobile() {
    var d = navigator.userAgent || navigator.vendor || window.opera;
    return /windows phone/i.test(d) || /android/i.test(d) || /iPad|iPhone|iPod/.test(d) && !window.MSStream ? !0 : !1
}
function is_touch_device() {
    return !!("ontouchstart"in window)
}
window.requestAnimFrame = function() {
    return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || function(d) {
        window.setTimeout(d, 1E3 / 60)
    }
}();
function is_ie() {
    var d = window.navigator.userAgent
      , a = d.indexOf("MSIE ");
    if (0 < a)
        return parseInt(d.substring(a + 5, d.indexOf(".", a)), 10);
    if (0 < d.indexOf("Trident/"))
        return a = d.indexOf("rv:"),
        parseInt(d.substring(a + 3, d.indexOf(".", a)), 10);
    a = d.indexOf("Edge/");
    return 0 < a ? parseInt(d.substring(a + 5, d.indexOf(".", a)), 10) : !1
}
function is_ie11() {
    return !window.ActiveXObject && "ActiveXObject"in window
}
function version_ie() {
    return parseFloat(navigator.appVersion.split("MSIE")[1])
}
jQuery(document).ready(function(d) {
    d(".dzsparallaxer---window-height").each(function() {
        function a() {
            e.outerHeight(window.innerHeight)
        }
        var e = d(this);
        d(window).on("resize", a);
        a()
    });
    dzsprx_init(".dzsparallaxer.auto-init", {
        init_each: !0
    });
    console.log("holiprro")
});
