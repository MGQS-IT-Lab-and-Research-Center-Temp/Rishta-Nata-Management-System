/* Sarab - Fast Food & Restaurant Template Main JS */
(function ($) {
  "use strict";

  /* ---------- Navbar scroll effect ---------- */
  function navScroll() {
    var nav = $("#nav");
    if (!nav.length) return;
    if (window.scrollY > 40) {
      nav.addClass("scrolled");
    } else {
      nav.removeClass("scrolled");
    }
  }
  window.addEventListener("scroll", navScroll, { passive: true });
  navScroll();

  /* ---------- Back to top button ---------- */
  var btt = $("#btt");
  window.addEventListener(
    "scroll",
    function () {
      if (window.scrollY > 400) {
        btt.addClass("show");
      } else {
        btt.removeClass("show");
      }
    },
    { passive: true }
  );

  /* ---------- Mobile nav: collapse on link click ---------- */
  $("#navmenu a.nav-link").on("click", function () {
    if ($(".navbar-toggler").is(":visible")) {
      $("#navmenu").collapse("hide");
    }
  });

  /* ---------- Search overlay ---------- */
  var searchOv = $("#searchOv");

  function openSearch() {
    searchOv.addClass("open");
    $("#searchInput").trigger("focus");
  }
  function closeSearch() {
    searchOv.removeClass("open");
  }

  $("#navSearchBtn").on("click", openSearch);
  $("#searchClose").on("click", closeSearch);
  searchOv.on("click", function (e) {
    if (e.target === this) closeSearch();
  });
  $(document).on("keydown", function (e) {
    if (e.key === "Escape") {
      closeSearch();
      closeMenuPop();
      closeGalPop();
    }
  });

  function applyFilter(cat) {
    var $items = $("#mgrid .mwrap");
    if (!cat || cat === "all") {
      $items.removeClass("gone");
      return;
    }
    $items.each(function () {
      $(this).toggleClass("gone", $(this).data("c") !== cat);
    });
  }

  function setFiltActive(cat) {
    $(".filtbtn").removeClass("active");
    $(".filtbtn[data-f='" + cat + "']").addClass("active");
  }

  /* ---------- Category cards -> filter + scroll to menu ---------- */
  $(".catcard").on("click", function () {
    $(".catcard").removeClass("active");
    $(this).addClass("active");
    var cat = $(this).data("filter") || "all";
    applyFilter(cat);
    setFiltActive(cat);
    if ($("#menu").length) {
      $("html, body").animate({ scrollTop: $("#menu").offset().top - 70 }, 600);
    }
  });

  /* ---------- Menu filter buttons ---------- */
  $(".filtbtn").on("click", function () {
    var cat = $(this).data("f") || "all";
    setFiltActive(cat);
    applyFilter(cat);
  });

  /* ---------- Search overlay categories + input ---------- */
  $(".sovcat").on("click", function () {
    $(".sovcat").removeClass("active");
    $(this).addClass("active");
    applyFilter($(this).data("cat"));
    closeSearch();
    if ($("#menu").length) {
      $("html, body").animate({ scrollTop: $("#menu").offset().top - 70 }, 600);
    }
  });

  $("#searchInput").on("keydown", function (e) {
    if (e.key !== "Enter") return;
    var q = $.trim($(this).val()).toLowerCase();
    if (!q) return;
    var matched = false;
    $("#mgrid .mwrap").each(function () {
      var $w = $(this);
      var $c = $w.find(".mtit");
      var $d = $w.find(".mdesc");
      var hay = ($c.text() + " " + $d.text()).toLowerCase();
      if (hay.indexOf(q) !== -1) {
        $w.removeClass("gone");
        matched = true;
      } else {
        $w.addClass("gone");
      }
    });
    if (!matched) {
      applyFilter("all");
    }
    setFiltActive("all");
    $(".sovcat").removeClass("active");
    $('.sovcat[data-cat="all"]').addClass("active");
    closeSearch();
    if ($("#menu").length) {
      $("html, body").animate({ scrollTop: $("#menu").offset().top - 70 }, 600);
    }
  });

  /* ---------- Menu detail popup ---------- */
  function starsHTML(rating) {
    var full = Math.round(parseFloat(rating) || 0);
    var html = "";
    for (var i = 0; i < 5; i++) {
      html += i < full ? '<i class="fas fa-star"></i>' : '<i class="far fa-star"></i>';
    }
    return html;
  }

  function resolveUrl(p) {
    if (!p) return p;
    if (/^(https?:)?\/\//i.test(p) || p.charAt(0) === "/") return p;
    return (window.AppRoot || "/") + p;
  }

  var menuPop = $("#menuPop");
  var currentMCard = null;

  function openMenuPop(card) {
    currentMCard = $(card);
    var d = currentMCard;
    var img = resolveUrl(d.attr("data-img")) || "";
    var title = d.attr("data-title") || "";
    var cat = d.attr("data-cat") || "";
    var price = d.attr("data-price") || "";
    var old = d.attr("data-old") || "";
    var rating = d.attr("data-rating") || "5";
    var reviews = d.attr("data-reviews") || "0";
    var cal = d.attr("data-cal") || "0";
    var time = d.attr("data-time") || "0";
    var desc = d.attr("data-desc") || "";
    var tags = (d.attr("data-tags") || "").split(",").filter(Boolean);

    $("#mpImg").attr("src", img);
    $("#mpCat").text(cat);
    $("#mpTitle").text(title);
    $("#mpStars").html(starsHTML(rating) + ' <span style="color:#bbb;font-size:.78rem;">' + rating + ' (' + reviews + ' reviews)</span>');
    $("#mpDesc").text(desc);
    $("#mpPrice").html(
      price + (old ? " <small>" + old + "</small>" : "")
    );
    $("#mpMeta").html(
      '<div class="mpm"><div class="mpmv">' +
        cal +
        " kcal</div><div class=\"mpml\">Calories</div></div>" +
        '<div class="mpm"><div class="mpmv">' +
        time +
        ' min</div><div class="mpml">Prep Time</div></div>' +
        '<div class="mpm"><div class="mpmv">' +
        rating +
        '</div><div class="mpml">Rating</div></div>'
    );
    var tagHtml = "";
    for (var i = 0; i < tags.length; i++) {
      tagHtml += '<span class="mptag">' + tags[i] + "</span>";
    }
    $("#mpTags").html(tagHtml);
    $("#mpQnum").text("1");
    menuPop.addClass("open");
  }
  function closeMenuPop() {
    menuPop.removeClass("open");
    currentMCard = null;
  }

  $(document).on("click", ".mcard", function (e) {
    if ($(e.target).closest(".madd, .mhrt").length) return;
    openMenuPop(this);
  });
  $(document).on("click", ".madd", function (e) {
    e.stopPropagation();
    openMenuPop($(this).closest(".mcard"));
  });
  $(document).on("click", ".mhrt", function (e) {
    e.stopPropagation();
    var $i = $(this).find("i");
    $i.toggleClass("fas far");
    $(this).css("color", $i.hasClass("fas") ? "var(--primary)" : "#ccc");
  });

  $("#mpClose").on("click", closeMenuPop);
  menuPop.on("click", function (e) {
    if (e.target === this) closeMenuPop();
  });

  $("#mpMinus").on("click", function () {
    var n = parseInt($("#mpQnum").text(), 10) || 1;
    $("#mpQnum").text(Math.max(1, n - 1));
  });
  $("#mpPlus").on("click", function () {
    var n = parseInt($("#mpQnum").text(), 10) || 1;
    $("#mpQnum").text(n + 1);
  });
  $("#mpAddCart").on("click", function () {
    closeMenuPop();
  });

  /* ---------- Gallery popup ---------- */
  var galPop = $("#galPop");
  var galItems = $(".gitem").toArray();
  var galIndex = 0;

  function showGal(i) {
    if (!galItems.length) return;
    galIndex = (i + galItems.length) % galItems.length;
    var item = $(galItems[galIndex]);
    $("#gpImg").attr("src", resolveUrl(item.attr("data-gimg") || ""));
    $("#gpTitle").text(item.attr("data-gtitle") || "");
    $("#gpDesc").text(item.attr("data-gdesc") || "");
    galPop.addClass("open");
  }
  function closeGalPop() {
    galPop.removeClass("open");
  }

  $(document).on("click", ".gitem", function () {
    showGal($(this).data("gi"));
  });
  $("#gpClose").on("click", closeGalPop);
  galPop.on("click", function (e) {
    if (e.target === this) closeGalPop();
  });
  $("#gpPrev").on("click", function () {
    showGal(galIndex - 1);
  });
  $("#gpNext").on("click", function () {
    showGal(galIndex + 1);
  });

  /* ---------- Special offer countdown ---------- */
  function startCountdown() {
    var els = { h: $("#cdH"), m: $("#cdM"), s: $("#cdS") };
    if (!els.h.length) return;
    var target = Date.now() + 2 * 24 * 60 * 60 * 1000 + 8 * 60 * 60 * 1000;
    var pad = function (n) {
      return n < 10 ? "0" + n : "" + n;
    };
    setInterval(function () {
      var diff = target - Date.now();
      if (diff < 0) target = Date.now() + 2 * 24 * 60 * 60 * 1000;
      var h = Math.floor(diff / 3600000);
      var m = Math.floor((diff % 3600000) / 60000);
      var s = Math.floor((diff % 60000) / 1000);
      els.h.text(pad(h));
      els.m.text(pad(m));
      els.s.text(pad(s));
    }, 1000);
  }
  startCountdown();

  /* ---------- Reservation / Contact / Newsletter (demo) ---------- */
  $("#resBtn").on("click", function (e) {
    e.preventDefault();
    $("#resOk").fadeIn();
  });
  $("#ctcBtn").on("click", function (e) {
    e.preventDefault();
    $("#ctcOk").fadeIn();
  });
  $("#nlBtn").on("click", function () {
    var email = $.trim($("#nlEmail").val());
    if (!email) {
      $("#nlEmail").trigger("focus");
      return;
    }
    var $btn = $(this);
    $btn.html('<i class="fas fa-check"></i> Subscribed!');
    setTimeout(function () {
      $btn.html('<i class="fas fa-paper-plane me-1"></i>Subscribe');
    }, 2500);
    $("#nlEmail").val("");
  });

  /* ---------- Magnific popup (video) ---------- */
  if ($.fn.magnificPopup) {
    $(".popup-youtube").magnificPopup({
      type: "iframe",
      mainClass: "mfp-fade",
      removalDelay: 160,
      preloader: false,
      fixedContentPos: false,
    });
  }

  /* ---------- Swiper testimonials ---------- */
  if (window.Swiper && $(".tesSwiper").length) {
    new Swiper(".tesSwiper", {
      loop: true,
      spaceBetween: 24,
      slidesPerView: 1,
      grabCursor: true,
      autoplay: { delay: 4500, disableOnInteraction: false },
      pagination: {
        el: ".tesSwiper .swiper-pagination",
        clickable: true,
      },
      breakpoints: {
        768: { slidesPerView: 2 },
        1200: { slidesPerView: 3 },
      },
    });
  }

  /* ---------- AOS animations ---------- */
  if (window.AOS) {
    AOS.init({
      duration: 700,
      offset: 60,
      once: true,
    });
  }
})(jQuery);
