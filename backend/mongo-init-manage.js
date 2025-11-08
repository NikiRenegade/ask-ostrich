db = db.getSiblingDB('ao_survey_manage');
db.createUser({
    user: 'user',
    pwd: 'password',
    roles: [
        { role: 'readWrite', db: 'ao_survey_manage' }
    ]
});
