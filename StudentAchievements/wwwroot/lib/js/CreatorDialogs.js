$(function () {
    $('.editVacancy-container').click(function () {
        var id = $(this).attr('data-id');
    fetch("/Vacancy/EditVacancy/?id=" + id)
      .then((response) => {
        return response.text();
      })
      .then((result) => {
        $('#editVacancy-content-container').html(result);
                $('#editVacancy-container').modal('show');
      });
    });
});

$(function () {
    $('.removeVacancy-container').click(function () {
        var id = $(this).attr('data-id');
    fetch("/Vacancy/ShowRemoveVacancy/?id=" + id)
      .then((response) => {
        return response.text();
      })
      .then((result) => {
        $('#removeVacancy-content-container').html(result);
                $('#removeVacancy-container').modal('show');
      });
    });
});

$(function () {
  $('.addVacancy-container').click(function () {
  fetch("/Vacancy/AddVacancy")
    .then((response) => {
      return response.text();
    })
    .then((result) => {
      $('#addVacancy-content-container').html(result);
              $('#addVacancy-container').modal('show');
    });
  });
});