% --------------------
% Script to save GameObject data to separate csv-files to replay
% 
% 2021 - Yves Onnink
% --------------------

% General cleanup

close all; clear all; clc;

% create empty GameObjects holder

gameobjects = [];

%% Import raw data

subjectnumber = input('Subject number: '); % Identifier for the subject
sessionnumber = input('Session number: '); % Identifier for the session (each day is another session)
trialnumber = input('Trial number: '); % Identifier for the trial
filename = "RAW_Subject" + int2str(subjectnumber) + "_Session" + int2str(sessionnumber) + "_Trial" + int2str(trialnumber) + ".mat";

data = importdata(filename);

%% Create separate matrices for each GameObject

nr_gameobjects = (size(data,2) - 1) / 6;

timestamp = data(:,end);
timestamp = timestamp - timestamp(1); % make the recording "start" from 0

for ii = 0 : nr_gameobjects-1
    gameobjects(:,:,ii+1) = [data(:,(ii*6)+1:(ii+1)*6) timestamp];
end

%% Save to csv-files with a header

header = {'pos_x' 'pos_y' 'pos_z' 'rot_x' 'rot_y' 'rot_z' 'time'};
header = strjoin(header,',');

for jj = 1 : nr_gameobjects
    object_filename = "GameObject" + int2str(jj) + ".csv";
    fid = fopen(object_filename,'w'); fprintf(fid,'%s\n',header); fclose(fid);
    dlmwrite(object_filename, gameobjects(:,:,jj), '-append');
end