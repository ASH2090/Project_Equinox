jQuery.validator.addMethod("minimumage", function(value, element, param) {
	if (value === '') return false;

	var dob = new Date(value);
	if (isNaN(dob.getTime())) return false;

	var minYears = Number(param.years);
	var maxYears = (param.maxyears !== undefined && param.maxyears !== null && param.maxyears !== "") ? Number(param.maxyears) : null;

	var today = new Date();
	var age = today.getFullYear() - dob.getFullYear();
	var m = today.getMonth() - dob.getMonth();
	if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
		age--;
	}

	if (age < minYears) return false;
	if (maxYears !== null && age > maxYears) return false;
	return true;
});

jQuery.validator.unobtrusive.adapters.addSingleVal("minimumage", "years");
