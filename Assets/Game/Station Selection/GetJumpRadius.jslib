mergeInto(LibraryManager.library, {
  GetJumpRadius: function () {
    return parseFloat(new URLSearchParams(window.location.search).get('jr'));
  }
});
