# KeelHaul
  Only Purple ideas allowed


#Making changes to the repo
With the new Pull request system, there are some steps to follow to contribute code to the master branch

Whenever you are working on a new feature or bug, do the following steps:
 1. Update your master branch to the latest (git pull origin master)
 2. `git branch name-of-feature`
 3. Switch to that branch and do your work (`git checkout name-of-feature`)
 4. After finishing up, `git push origin name-of-feature`
 5. Create a pull request to the master branch and wait for it to be reviewed 
 
 
#Review process
General guidelines to keep in mind

 1. Artists will be reviewed by me and any one other programmer
 2. Every programmer will be reviewed by the other two programmers
 3. When a pull request has 2 successful reviews, tag the PR as "Two Thumbs", so I can come in and merge it to master
 4. Don't merge to master yourself, Dont push to master directly. I can enforce these restrctions, but I would like to allow the freedom for emergency purposes

#Commiting 
 1. Lets all try not to use git add* 
 2. git add and commit per commit unless the files are related to the change
 3. Any prefab changes especially needs to be commented to show what exactly changed
 4. Design changes like ship speed, will only be committed by the person who's laptop we worked on during the meeting (Most likely Vig)
 
 
 #Shipyard:
  Every once in a while, a build considered stable will be pushed to the shipyard branch, usually after big sprints like IGF
  Everyone can checkout to this branch if they want a stable build to show 
