(function (window) {
  'use strict';

  function init() {
    var yes = document.getElementById('IsMemberYes');
    var no = document.getElementById('IsMemberNo');
    var wrap = document.getElementById('membershipWrap');
    var membership = document.getElementById('MemberMembershipNo');
    var tokenInput = document.getElementById('Token');
    var notice = document.getElementById('lookupNotice');

    if (!yes || !no || !wrap || !membership || !tokenInput) { return; }

    function refresh() {
      wrap.style.display = yes.checked ? '' : 'none';
    }

    function lookup() {
      if (!yes.checked) { return; }
      var chandaNo = (membership.value || '').trim();
      var token = (tokenInput.value || '').trim();
      if (!chandaNo || !token) { return; }

      if (notice) { notice.classList.add('d-none'); }

      var url = '/SharedSection/MemberLookup?token=' + encodeURIComponent(token) +
                '&chandaNo=' + encodeURIComponent(chandaNo);

      fetch(url)
        .then(function (res) {
          if (!res.ok) {
            if (res.status === 404 && notice) {
              notice.textContent = 'That membership number was not found. Please enter your details manually.';
              notice.classList.remove('d-none');
            }
            return null;
          }
          return res.json();
        })
        .then(function (m) {
          if (!m) { return; }
          var n = document.getElementById('Name');
          var a = document.getElementById('Address');
          var t = document.getElementById('Tel');
          if (n) { n.value = m.fullName || ''; }
          if (a) { a.value = m.address || ''; }
          if (t) { t.value = m.phoneNo || ''; }
        })
        .catch(function () {
          if (notice) {
            notice.textContent = 'Could not load your details. Please enter them manually.';
            notice.classList.remove('d-none');
          }
        });
    }

    membership.addEventListener('change', lookup);
    membership.addEventListener('blur', lookup);
    yes.addEventListener('change', refresh);
    no.addEventListener('change', refresh);
    refresh();
  }

  document.addEventListener('DOMContentLoaded', init);
})(window);
