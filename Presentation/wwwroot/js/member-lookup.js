// member-lookup.js — membership-number -> member detail auto-fill (no login),
// plus visibility toggling of the membership-number field per party.
(function (window) {
  'use strict';

  function lookupAndFill(membershipInput, config) {
    var chandaNo = (membershipInput.value || '').trim();
    if (!chandaNo) { return; }

    fetch('/api/members/lookup/' + encodeURIComponent(chandaNo))
      .then(function (res) {
        if (!res.ok) { return; } // 404 => free-text entry
        return res.json();
      })
      .then(function (m) {
        if (!m) { return; }
        if (config.nameId) {
          var n = document.getElementById(config.nameId);
          if (n) { n.value = m.fullName || ''; }
        }
        if (config.addressId) {
          var a = document.getElementById(config.addressId);
          if (a) { a.value = m.address || ''; }
        }
        if (config.telId) {
          var t = document.getElementById(config.telId);
          if (t) { t.value = m.phoneNo || ''; }
        }
      })
      .catch(function (err) { console.warn('Member lookup failed', err); });
  }

  // config: { checkboxId, wrapId, membershipId, nameId, addressId, telId }
  function initParty(config) {
    var box = document.getElementById(config.checkboxId);
    if (!box) { return; }

    var wrap = document.getElementById(config.wrapId);
    var membershipInput = document.getElementById(config.membershipId);
    var nameEl = config.nameId ? document.getElementById(config.nameId) : null;

    if (membershipInput) {
      membershipInput.addEventListener('change', function () {
        lookupAndFill(membershipInput, config);
      });
    }

    function refresh() {
      var isMember = box.checked;
      if (wrap) { wrap.style.display = isMember ? '' : 'none'; }
      if (membershipInput) { membershipInput.required = isMember; }
      if (nameEl) { nameEl.required = !isMember; }
    }

    box.addEventListener('change', refresh);
    refresh();
  }

  window.RNMemberLookup = { initParty: initParty };
})(window);
