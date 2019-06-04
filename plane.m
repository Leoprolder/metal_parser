function res=plane(strfunc,vx0,vx1,vy0,vy1,h)
vx=vx0:h:vx1;
vy=vy0:h:vy1;
figure(1)
res=ezsurfc(strfunc,vx,vy);
end