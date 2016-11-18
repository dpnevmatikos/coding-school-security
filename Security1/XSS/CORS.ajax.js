$.ajax({
    url:'http://localhost:9177/Person/Update',
    type: 'post',
    crossDomain: true,
    dataType: 'jsonp',
    xhrFields: {
        withCredentials: true
    },
    data: {
        Id: 1,
        Name: 'Jim',
        Password: 'test123'
    }
}).done(function() {
    alert('success');
}).fail(function() {
    alert('fail');
});
