# NTI-Admin

## For Developers

### For setup (beggining of day)
run the setup.sh script. This will install and / or update all dependicys and required programs, run docker, setup the db and the rest of the project. 

### Seed the database
This is not required if you've ran the setup.sh script before, but could be needed if you have done things and fucked up your database content. 

Just run the seed.sh script, this will give you two options. Option 2 will seed the entire database with content from the Google API, which is needed for full seeding but don't use it too often since Google will be mad at us if so. If your database already contains the users, just choose option 1. Skibidi. 

### Run the test
You could run some tests with the test.sh script. This will run both the backend and frontend tests, but if you want to run one specific test you could instead run test_frontend.sh or test_backend.sh. 