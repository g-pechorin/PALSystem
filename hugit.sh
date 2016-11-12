
# make the dir if needed
if [ ! -d .hugit/ ]; then
  git clone git@github.com:g-pechorin/PALSystem.git
  mv PALSystem/.git .hugit
  rm -dfr PALSystem
fi;

#
git --git-dir=`pwd`/.hugit/ $*
